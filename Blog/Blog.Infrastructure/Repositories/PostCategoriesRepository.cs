using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal class PostCategoriesRepository : BaseRepository<PostCategory>, IPostCategoriesRepository
{
    public PostCategoriesRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task AddEntitiesAsync(PostCategory[] entities, CancellationToken cancellationToken)
        => await _set.AddRangeAsync(entities, cancellationToken);

    public async Task DeleteAllForPostAsync(int postId, CancellationToken cancellationToken)
        => await _set.Where(e => e.PostId == postId)
                     .ExecuteDeleteAsync(cancellationToken);

    public async Task DeleteInstancesForPostAsync(int postId, int[] categoryIds, CancellationToken cancellationToken)
        => await _set.Where(e => e.PostId == postId && categoryIds.Contains(e.CategoryId))
                     .ExecuteDeleteAsync(cancellationToken);

    public async Task<bool> DoesCategoryHavePostsAsync(int categoryId, CancellationToken cancellationToken)
        => await _untrackedSet.Where(e => e.CategoryId == categoryId)
                              .AnyAsync(cancellationToken);

    public async Task<IEnumerable<PostCategory>> GetAllForPostAsync(int postId, CancellationToken cancellationToken)
        => await _untrackedSet.Where(e => e.PostId == postId)
                              .ToListAsync(cancellationToken);
}
