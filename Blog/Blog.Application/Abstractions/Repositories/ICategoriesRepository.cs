using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Repositories;

public interface ICategoriesRepository : IBaseIntRepository<Category>
{
    Task<bool> DoesNameExistAsync(string name, CancellationToken cancellationToken = default);
}
