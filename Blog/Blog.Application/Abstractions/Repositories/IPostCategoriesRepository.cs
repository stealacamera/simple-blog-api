using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface IPostCategoriesRepository : IBaseRepository<PostCategory>
{
    Task<bool> DoesCategoryHavePostsAsync(int categoryId, CancellationToken cancellationToken);
}
