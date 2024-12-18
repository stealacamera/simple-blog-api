using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Repositories;
using Blog.Infrastructure.Repositories;

namespace Blog.Infrastructure;

internal sealed class WorkUnit : IWorkUnit
{
    private readonly AppDbContext _dbContext;

    public WorkUnit(AppDbContext dbContext)
        => _dbContext = dbContext;
    public async Task SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();

    // Repositories
    private IUsersRepository _usersRepository = null!;
    public IUsersRepository UsersRepository
    {
        get
        {
            _usersRepository ??= new UsersRepository(_dbContext);
            return _usersRepository;
        }
    }

    private ICategoriesRepository _categoriesRepository = null!;
    public ICategoriesRepository CategoriesRepository
    {
        get
        {
            _categoriesRepository ??= new CategoriesRepository(_dbContext); 
            return _categoriesRepository;
        }
    }

    private IPostCategoriesRepository _postCategoriesRepository = null!;
    public IPostCategoriesRepository PostCategoriesRepository
    {
        get
        {
            _postCategoriesRepository ??= new PostCategoriesRepository(_dbContext); 
            return _postCategoriesRepository;
        }
    }
}
