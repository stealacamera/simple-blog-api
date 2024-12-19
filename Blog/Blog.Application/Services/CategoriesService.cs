using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Services;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Requests.CategoryRequests;
using FluentValidation;
using FluentValidation.Results;

namespace Blog.Application.Services;

internal sealed class CategoriesService : BaseService, ICategoriesService
{
    public CategoriesService(IWorkUnit workUnit) : base(workUnit)
    {
    }

    public async Task<IList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _workUnit.CategoriesRepository
                                        .GetAllAsync(cancellationToken);

        return categories.Select(e => new Category(e.Id, e.Name, e.Description))
                         .ToList();
    }

    public async Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        // Validate name uniqeness
        if (await _workUnit.CategoriesRepository.DoesNameExistAsync(request.Name, cancellationToken))
            throw new ValidationException([new ValidationFailure(nameof(request.Name), "Category name already exists")]);

        // Create new category
        var category = new Domain.Entities.Category
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _workUnit.CategoriesRepository.AddAsync(category, cancellationToken);
        await _workUnit.SaveChangesAsync();

        return new Category(category.Id, category.Name, category.Description);
    }

    public async Task<Category> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _workUnit.CategoriesRepository
                                      .GetByIdAsync(id, cancellationToken);
        // Check if entity exists
        if (category == null)
            throw new EntityNotFoundException(nameof(Category));

        // Change attributes if given
        if (request.Name != null)
        {
            if (await _workUnit.CategoriesRepository.DoesNameExistAsync(request.Name))
                throw new ValidationException([new ValidationFailure(nameof(request.Name), "Category name already exists")]);

            category.Name = request.Name;
        } 

        if(request.Description != null)
            category.Description = request.Description;

        await _workUnit.SaveChangesAsync();
        return new Category(category.Id, category.Name, category.Description);
    }
    
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _workUnit.CategoriesRepository
                                      .GetByIdAsync(id, cancellationToken);

        if(category == null)
            throw new EntityNotFoundException(nameof(Category));

        // Don't proceed if category has posts
        if (await _workUnit.PostCategoriesRepository.DoesCategoryHavePostsAsync(id, cancellationToken))
            throw new CategoryHasPostsException();

        _workUnit.CategoriesRepository.Delete(category);
        await _workUnit.SaveChangesAsync();
    }
}
