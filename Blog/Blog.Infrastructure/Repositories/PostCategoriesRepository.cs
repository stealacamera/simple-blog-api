using Blog.Application.Abstractions.Services;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal class PostCategoriesRepository : BaseRepository<PostCategory>, IPostCategoriesRepository
{
    public PostCategoriesRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<bool> DoesCategoryHavePostsAsync(int categoryId, CancellationToken cancellationToken)
        => await _untrackedSet.Where(e => e.CategoryId == categoryId)
                              .AnyAsync(cancellationToken);
}
