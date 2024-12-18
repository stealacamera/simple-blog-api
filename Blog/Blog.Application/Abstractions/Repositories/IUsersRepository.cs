using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface IUsersRepository : IBaseIntRepository<User>
{
    Task<bool> DoesUsernameExistAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default);
}
