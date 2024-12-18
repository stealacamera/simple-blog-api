using Blog.Application.Common.DTOs;
using Blog.Application.Common.Requests;

namespace Blog.Application.Abstractions.Services;

public interface ICategoriesService
{
    Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default);
}
