using Blog.Application.Abstractions.Repositories;
using Blog.Application.Abstractions.Services;

namespace Blog.Application.Abstractions;

public interface IWorkUnit
{
    Task SaveChangesAsync();

    // Repositories
    IUsersRepository UsersRepository { get; }
    ICategoriesRepository CategoriesRepository { get; }
    IPostCategoriesRepository PostCategoriesRepository { get; }
}
