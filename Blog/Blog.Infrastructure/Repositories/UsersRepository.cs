using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal sealed class UsersRepository : BaseIntRepository<User>, IUsersRepository
{
    public UsersRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DoesEmailExistAsync(string email, CancellationToken cancellationToken = default)
        => await _untrackedSet.Where(e => e.Email == email)
                              .AnyAsync(cancellationToken);

    public async Task<bool> DoesUsernameExistAsync(string username, CancellationToken cancellationToken = default)
        => await _untrackedSet.Where(e => e.Username == username)
                              .AnyAsync(cancellationToken);
}
