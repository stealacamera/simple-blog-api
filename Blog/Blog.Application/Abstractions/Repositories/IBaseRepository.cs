using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Delete(T entity);
}

public interface IBaseSimpleRepository<TEntity, TKey> : IBaseRepository<TEntity> 
    where TEntity : BaseSimpleEntity<TKey> 
    where TKey : struct, IComparable<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<bool> DoesInstanceExistAsync(TKey id, CancellationToken cancellationToken = default);
}

public interface IBaseIntRepository<T> : IBaseSimpleRepository<T, int>
    where T : BaseIntEntity
{ }