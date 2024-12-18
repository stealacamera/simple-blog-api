using Blog.Application.Abstractions;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : BaseController
{
    public CategoriesController(IServicesManager servicesManager) : base(servicesManager) { }

    [HttpPost]
    public async Task<Created<Category>> CreateAsync(
        CreateCategoryRequest request, 
        IValidator<CreateCategoryRequest> validator, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var newCategory = await _servicesManager.CategoriesService.CreateAsync(request, cancellationToken);
        return TypedResults.Created(string.Empty, newCategory);
    }
}
