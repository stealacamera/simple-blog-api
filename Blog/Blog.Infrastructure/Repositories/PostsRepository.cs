using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Common.Enums;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal sealed class PostsRepository : BaseIntRepository<Post>, IPostsRepository
{
    public PostsRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<Post>> GetAllAsync(
        string? filterByTitle = null, 
        string? filterByContent = null,
        PostStatuses? filterByStatus = null,
        DateOnly? filterByPublicationDate = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _untrackedSet;

        if(filterByTitle != null)
            query = query.Where(e => e.Title.Contains(filterByTitle));
        if(filterByContent != null)
            query = query.Where(e => e.Content.Contains(filterByContent));

        if(filterByStatus != null)
            query = query.Where(e => e.PostStatusId == filterByStatus.Id);

        if(filterByPublicationDate.HasValue)
            query = query.Where(
                            e => e.PublishedAt.HasValue && 
                            DateOnly.FromDateTime(e.PublishedAt.Value) == filterByPublicationDate.Value);

        return await query.ToListAsync(cancellationToken);
    }
}
