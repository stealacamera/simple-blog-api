using Blog.Application.Abstractions.Services;

namespace Blog.Application.Abstractions;

public interface IServicesManager
{
    IUsersService UsersService { get; }
    ICategoriesService CategoriesService { get; }
}
