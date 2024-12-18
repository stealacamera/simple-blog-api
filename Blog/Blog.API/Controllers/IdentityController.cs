using Blog.Application.Abstractions;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : BaseController
{
    public IdentityController(IServicesManager servicesManager) : base(servicesManager) { }

    [HttpPost]
    public async Task<Created<User>> RegisterAsync(
        CreateUserRequest request, 
        IValidator<CreateUserRequest> validator, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await _servicesManager.UsersService.CreateAsync(request, cancellationToken);
        return TypedResults.Created(string.Empty, user);
    }
}
