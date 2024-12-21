using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Services;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.DTOs.Requests.CategoryRequests;
using Blog.Application.Common.Exceptions;
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
        return (await _workUnit.CategoriesRepository
                               .GetAllAsync(cancellationToken))
                               .Select(e => new Category(e.Id, e.Name, e.Description))
                               .ToList();
    }

    public async Task<Category> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        await ValidateCategoryNameUniquenessAsync(request.Name, nameof(request.Name), cancellationToken);

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
            await ValidateCategoryNameUniquenessAsync(request.Name, nameof(request.Name), cancellationToken);
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
        else if (await _workUnit.PostCategoriesRepository.DoesCategoryHavePostsAsync(id, cancellationToken))
            throw new CategoryHasPostsException(); // Don't proceed if category has posts

        _workUnit.CategoriesRepository.Delete(category);
        await _workUnit.SaveChangesAsync();
    }


    // Helper methods
    private async Task ValidateCategoryNameUniquenessAsync(string name, string propertyName, CancellationToken cancellationToken)
    {
        if (await _workUnit.CategoriesRepository.DoesNameExistAsync(name))
            throw new ValidationException([new ValidationFailure(nameof(propertyName), "Category name already exists")]);
    }
}
