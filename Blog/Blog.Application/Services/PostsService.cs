using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Services;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Requests;
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

        return new Post(
            newPost.Id, newPost.Title, newPost.Content,
            new PostStatus(request.Status), newPost.CreatedAt, newPost.PublishedAt,
            postCategories);
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
