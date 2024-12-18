using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal sealed class CategoriesRepository : BaseIntRepository<Category>, ICategoriesRepository
{
    public CategoriesRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<bool> DoesNameExistAsync(string name, CancellationToken cancellationToken = default)
        => await _untrackedSet.Where(e => e.Name == name)
                              .AnyAsync(cancellationToken);

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _untrackedSet.ToListAsync(cancellationToken);
}   
