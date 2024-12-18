using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface ICategoriesRepository : IBaseIntRepository<Category>
{
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DoesNameExistAsync(string name, CancellationToken cancellationToken = default);
}
