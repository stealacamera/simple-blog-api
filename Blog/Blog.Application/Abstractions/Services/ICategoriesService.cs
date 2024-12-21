using Blog.Application.Common.DTOs;
using Blog.Application.Common.DTOs.Requests.CategoryRequests;

namespace Blog.Application.Abstractions.Services;

public interface ICategoriesService
{
    Task<IList<Category>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
    Task<Category> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
