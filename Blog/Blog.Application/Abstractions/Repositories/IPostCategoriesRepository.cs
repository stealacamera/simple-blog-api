using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface IPostCategoriesRepository : IBaseRepository<PostCategory>
{
    Task AddEntitiesAsync(PostCategory[] entities, CancellationToken cancellationToken);

    Task<IEnumerable<PostCategory>> GetAllForPostAsync(int postId, CancellationToken cancellationToken);
    Task<bool> DoesCategoryHavePostsAsync(int categoryId, CancellationToken cancellationToken);

    Task DeleteInstancesForPostAsync(int postId, int[] categoryIds, CancellationToken cancellationToken);
    Task DeleteAllForPostAsync(int postId, CancellationToken cancellationToken);
}
