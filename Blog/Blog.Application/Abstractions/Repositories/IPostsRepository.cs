using Blog.Domain.Common.Enums;
using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface IPostsRepository : IBaseIntRepository<Post>
{
    Task<IEnumerable<Post>> GetAllAsync(
        string? filterByTitle = null,
        string? filterByContent = null,
        PostStatuses? filterByStatus = null,
        DateOnly? filterByPublicationDate = null,
        CancellationToken cancellationToken = default);
}
