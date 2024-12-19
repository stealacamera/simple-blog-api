using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface IPostCategoriesRepository : IBaseRepository<PostCategory>
{
    Task<IEnumerable<PostCategory>> GetAllForPostAsync(int postId, CancellationToken cancellationToken);
    Task<bool> DoesCategoryHavePostsAsync(int categoryId, CancellationToken cancellationToken);
}
