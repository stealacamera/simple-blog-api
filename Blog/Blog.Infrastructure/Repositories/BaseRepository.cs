using Blog.Application.Abstractions.Repositories;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

internal abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _set;
    protected readonly IQueryable<T> _untrackedSet;

    protected BaseRepository(AppDbContext dbContext)
    {
        _set = dbContext.Set<T>();
        _untrackedSet = _set.AsNoTracking();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _set.AddAsync(entity, cancellationToken);

    public void Delete(T entity)
        => _set.Remove(entity);
}

internal abstract class BaseSimpleRepository<TEntity, TKey> : BaseRepository<TEntity>, IBaseSimpleRepository<TEntity, TKey>
    where TEntity : BaseSimpleEntity<TKey>
    where TKey : struct, IComparable<TKey>
{
    protected BaseSimpleRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<bool> DoesInstanceExistAsync(TKey id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken) != null;

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        => await _set.FindAsync(id, cancellationToken);
}

internal abstract class BaseIntRepository<T> : BaseSimpleRepository<T, int>, IBaseIntRepository<T>
    where T : BaseIntEntity
{
    protected BaseIntRepository(AppDbContext dbContext) : base(dbContext) { }
}