using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Services;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Requests.PostRequests;
using Blog.Domain.Common.Enums;

namespace Blog.Application.Services;

internal sealed class PostsService : BaseService, IPostsService
{
    public PostsService(IWorkUnit workUnit) : base(workUnit) { }

    public async Task<Post> CreateAsync(int requesterId, CreatePostRequest request, CancellationToken cancellationToken = default)
    {
        var categories = await ValidateRequestAsync(requesterId, request, cancellationToken);
        IList<Category> postCategories = new List<Category>();

        var newPost = new Domain.Entities.Post
        {
            Title = request.Title,
            Content = request.Content,
            OwnerId = requesterId,
            PostStatusId = request.Status.Id,
            CreatedAt = DateTime.UtcNow,
            PublishedAt = request.Status == PostStatuses.Public ? DateTime.UtcNow : null
        };

        await WrapInTransactionAsync(async () =>
        {
            await _workUnit.PostsRepository.AddAsync(newPost, cancellationToken);
            await _workUnit.SaveChangesAsync();

            postCategories = await RegisterPostCategoriesAsync(newPost.Id, categories, cancellationToken);
        });

        var requester = (await _workUnit.UsersRepository
                                        .GetByIdAsync(requesterId, cancellationToken))!;

        return new Post(
            newPost.Id, newPost.Title, newPost.Content,
            new PostStatus(request.Status), newPost.CreatedAt, newPost.PublishedAt,
            postCategories, new(requester.Id, requester.Username, requester.Email));
    }

    public async Task<IList<Post>> GetAllAsync(
        int? requesterId = null,
        string? filterByTitle = null,
        string? filterByContent = null,
        DateOnly? filterByPublicationDate = null,
        CancellationToken cancellationToken = default)
    {
        if (requesterId.HasValue)
        {
            // Validate user exists
            if (!await _workUnit.UsersRepository.DoesInstanceExistAsync(requesterId.Value, cancellationToken))
                throw new UnauthorizedException();
        }

        var posts = await _workUnit.PostsRepository
                                   .GetAllAsync(
                                        filterByTitle, filterByContent,
                                        /* Show only published posts to non-users */ requesterId.HasValue ? null : PostStatuses.Public,
                                        filterByPublicationDate, cancellationToken);

        return ConvertPostEntitiesAsync(posts, cancellationToken);
    }

    public async Task<PostDetails> UpdateAsync(int id, int requesterId, UpdatePostRequest request, CancellationToken cancellationToken = default)
    {
        var post = await ValidatePostAndOwnershipAsync(id, requesterId, cancellationToken);

        if (request.Title != null)
            post.Title = request.Title;
        if (request.Content != null)
            post.Content = request.Content;
        if (request.Status != null)
        {
            if (PostStatuses.FromId(post.PostStatusId) == PostStatuses.Deleted)
                throw new ChangeDeletedPostStatusException();

            post.PostStatusId = request.Status.Id;

            if (request.Status == PostStatuses.Public)
                post.PublishedAt = DateTime.UtcNow;
        }

        await _workUnit.SaveChangesAsync();

        var requester = (await _workUnit.UsersRepository
                                        .GetByIdAsync(requesterId, cancellationToken))!;

        return new PostDetails(
            post.Id, post.Title, post.Content,
            new(PostStatuses.FromId(post.PostStatusId)),
            post.CreatedAt, post.PublishedAt,
            new(requester.Id, requester.Username, requester.Email));
    }

    public async Task<Post> AddCategoriesToPostAsync(
        int id, 
        int requesterId, 
        AddCategoriesToPostRequest request, 
        CancellationToken cancellationToken = default)
    {
        var post = await ValidatePostAndOwnershipAsync(id, requesterId, cancellationToken);
        var postCategories = (await _workUnit.PostCategoriesRepository
                                            .GetAllForPostAsync(id, cancellationToken))
                                            .Select(e => e.CategoryId)
                                            .ToArray();

        // Retrieve existing post categories as DTOs and add as needed
        var categoriesDtos = (await _workUnit.CategoriesRepository
                                             .GetAllInstancesAsync(postCategories, cancellationToken))
                                             .Select(e => new Category(e.Id, e.Name, e.Description))
                                             .ToList();

        // If the category doesn't exist or is already attached to the post, do nothing
        // Else add category to post
        foreach(int newCategoryId in request.CategoryIds)
        {
            if (postCategories.Contains(newCategoryId))
                continue;

            var category = await _workUnit.CategoriesRepository
                                          .GetByIdAsync(newCategoryId, cancellationToken);

            if (category == null)
                continue;

            await _workUnit.PostCategoriesRepository
                           .AddAsync(new Domain.Entities.PostCategory 
                           { 
                               CategoryId = newCategoryId,
                               PostId = post.Id,
                               CreatedAt = DateTime.UtcNow,
                           });

            await _workUnit.SaveChangesAsync();
            categoriesDtos.Add(new(category.Id, category.Name, category.Description));
        }

        var requester = (await _workUnit.UsersRepository
                                        .GetByIdAsync(requesterId, cancellationToken))!;

        return new Post(
            post.Id, post.Title, post.Content, 
            new(PostStatuses.FromId(post.PostStatusId)), 
            post.CreatedAt, post.PublishedAt, 
            categoriesDtos, new(requester.Id, requester.Username, requester.Email));
    }


    // Helper methods
    private async Task<Domain.Entities.Post> ValidatePostAndOwnershipAsync(int id, int requesterId, CancellationToken cancellationToken)
    {
        // Validate post existence
        var post = await _workUnit.PostsRepository.GetByIdAsync(id, cancellationToken);

        if (post == null)
            throw new EntityNotFoundException(nameof(Post));

        // Validate post ownership
        var doesRequesterId = await _workUnit.UsersRepository
                                             .DoesInstanceExistAsync(requesterId, cancellationToken);

        if (!doesRequesterId || requesterId != post.OwnerId)
            throw new UnauthorizedException();

        return post;
    }

    private IList<Post> ConvertPostEntitiesAsync(
        IEnumerable<Domain.Entities.Post> entities, 
        CancellationToken cancellationToken)
    {
        Dictionary<int, Category> categoryDtos = new();
        Dictionary<int, User> postOwnersDtos = new();

        return entities.Select(async e =>
                        {
                            var postCategoriesDtos = new List<Category>();
                            var postCategoriesEntities = await _workUnit.PostCategoriesRepository
                                                                        .GetAllForPostAsync(e.Id, cancellationToken);

                            // Store category DTOs so as not to repeat requests to the database
                            foreach (var postCategory in postCategoriesEntities)
                            {
                                if (!categoryDtos.ContainsKey(postCategory.CategoryId))
                                {
                                    var category = (await _workUnit.CategoriesRepository
                                                                  .GetByIdAsync(postCategory.CategoryId, cancellationToken))!;

                                    categoryDtos.Add(
                                        postCategory.CategoryId,
                                        new(category.Id, category.Name, category.Description));
                                }

                                postCategoriesDtos.Add(categoryDtos[postCategory.CategoryId]);
                            }

                            // Store post owner DTOs so as not to repeat requests to the database
                            if (!postOwnersDtos.ContainsKey(e.OwnerId))
                            {
                                var owner = (await _workUnit.UsersRepository.GetByIdAsync(e.OwnerId))!;
                                postOwnersDtos.Add(e.OwnerId, new(owner.Id, owner.Username, owner.Email));
                            }

                            return new Post(
                                e.Id, e.Title, e.Content,
                                new(PostStatuses.FromId(e.PostStatusId)),
                                e.CreatedAt, e.PublishedAt, 
                                postCategoriesDtos, postOwnersDtos[e.OwnerId]);
                        })
                       .Select(e => e.Result)
                       .ToList();
    }

    private async Task<IEnumerable<Domain.Entities.Category>> ValidateRequestAsync(
        int requesterId,
        CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        // Validate requester
        if (!await _workUnit.UsersRepository.DoesInstanceExistAsync(requesterId, cancellationToken))
            throw new UnauthorizedException();

        // Ensure all categories exist
        var categories = await _workUnit.CategoriesRepository
                                        .GetAllInstancesAsync(request.CategoryIds.ToArray(), cancellationToken);

        if (categories.Count() < request.CategoryIds.Count)
        {
            var missingCategoryIds = request.CategoryIds
                                            .Except(categories.Select(e => e.Id).ToArray());

            throw new EntityNotFoundException($"Categories with id(s) {string.Join(", ", missingCategoryIds)}");
        }

        return categories;
    }

    private async Task<IList<Category>> RegisterPostCategoriesAsync(
        int postId,
        IEnumerable<Domain.Entities.Category> categories,
        CancellationToken cancellationToken)
    {
        List<Category> postCategories = new();

        foreach (var category in categories)
        {
            // Get category DTO
            postCategories.Add(new Category(category.Id, category.Name, category.Description));

            // Register post category entity
            var postCategory = new Domain.Entities.PostCategory
            {
                CategoryId = category.Id,
                PostId = postId,
                CreatedAt = DateTime.UtcNow,
            };

            await _workUnit.PostCategoriesRepository.AddAsync(postCategory, cancellationToken);
            await _workUnit.SaveChangesAsync();
        }

        return postCategories;
    }
}
