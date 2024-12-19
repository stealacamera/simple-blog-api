using Blog.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Application.Abstractions;

public interface IWorkUnit
{
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

    // Repositories
    IUsersRepository UsersRepository { get; }
    ICategoriesRepository CategoriesRepository { get; }
    IPostsRepository PostsRepository { get; }
    IPostCategoriesRepository PostCategoriesRepository { get; }
}
