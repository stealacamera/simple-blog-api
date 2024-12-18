using Blog.Application.Abstractions.Repositories;

namespace Blog.Application.Abstractions;

public interface IWorkUnit
{
    Task SaveChangesAsync();

    IUsersRepository UsersRepository { get; }
}
