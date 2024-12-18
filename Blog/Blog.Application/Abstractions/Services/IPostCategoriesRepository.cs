using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Services;

public interface IPostCategoriesRepository : IBaseRepository<PostCategory>
{
    Task<bool> DoesCategoryHavePostsAsync(int categoryId, CancellationToken cancellationToken);
}
