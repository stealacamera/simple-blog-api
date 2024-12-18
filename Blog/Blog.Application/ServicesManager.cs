using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Services;
using Blog.Application.Services;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application;

internal sealed class ServicesManager : IServicesManager
{
    private readonly IWorkUnit _workUnit;

    public ServicesManager(IWorkUnit workUnit)
        => _workUnit = workUnit;

    // Services
    private IUsersService _usersService = null!;
    public IUsersService UsersService
    {
        get
        {
            _usersService ??= new UsersService(_workUnit, new PasswordHasher<Domain.Entities.User>());
            return _usersService;
        }
    }

    private ICategoriesService _categoriesService = null!;
    public ICategoriesService CategoriesService
    {
        get
        {
            _categoriesService ??= new CategoriesService(_workUnit);
            return _categoriesService;
        }
    }
}
