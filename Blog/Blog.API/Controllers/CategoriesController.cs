using Blog.Application.Abstractions;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.DTOs.Requests.CategoryRequests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : BaseController
{
    public CategoriesController(IServicesManager servicesManager) : base(servicesManager) { }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation("Retrieves all existing categories")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IList<Category>))]
    public async Task<Ok<IList<Category>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _servicesManager.CategoriesService
                                               .GetAllAsync(cancellationToken);
        
        return TypedResults.Ok(categories);
    }

    [HttpPost]
    [SwaggerOperation("Create a new category")]
    [SwaggerResponse(StatusCodes.Status201Created, "New category was created successfully", typeof(Category))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Category name already exists, or request is invalid", typeof(ValidationProblemDetails))]
    public async Task<Created<Category>> CreateAsync(
        CreateCategoryRequest request, 
        IValidator<CreateCategoryRequest> validator, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var newCategory = await _servicesManager.CategoriesService.CreateAsync(request, cancellationToken);
        return TypedResults.Created(string.Empty, newCategory);
    }

    [HttpPatch("{id:int:min(1)}")]
    [SwaggerOperation("Update an existing category")]
    [SwaggerResponse(StatusCodes.Status200OK, "Category was updated successfully", typeof(Category))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category could not be found", typeof(ProblemDetails))]
    public async Task<Ok<Category>> UpdateAsync(
        int id,
        UpdateCategoryRequest request,
        IValidator<UpdateCategoryRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var updatedCategory = await _servicesManager.CategoriesService
                                                    .UpdateAsync(id, request, cancellationToken);
        
        return TypedResults.Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    [SwaggerOperation("Delete an existing category")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or existing posts are linked to the category", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category could not be found", typeof(ProblemDetails))]
    public async Task<NoContent> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _servicesManager.CategoriesService.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
