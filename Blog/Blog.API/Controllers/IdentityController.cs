using System.Security.Claims;
using Blog.Application.Abstractions;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.DTOs.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : BaseController
{
    public IdentityController(IServicesManager servicesManager) : base(servicesManager) { }

    [HttpPost("register")]
    [SwaggerOperation("Create user account")]
    [SwaggerResponse(StatusCodes.Status201Created, "Account created successfully", typeof(User))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    public async Task<Created<User>> RegisterAsync(
        CreateUserRequest request, 
        IValidator<CreateUserRequest> validator, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await _servicesManager.UsersService
                                         .CreateAsync(request, cancellationToken);
        
        return TypedResults.Created(string.Empty, user);
    }

    [HttpPost("login")]
    [SwaggerOperation("Log into account")]
    [SwaggerResponse(StatusCodes.Status200OK, "Logged into account successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or wrong credentials")]
    public async Task<Ok> LoginAsync(
        ValidateCredentialsRequest request, 
        IValidator<ValidateCredentialsRequest> validator, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request);

        var user = await _servicesManager.UsersService
                                         .ValidateCredentialsAsync(request, cancellationToken);

        await AuthenticateUserAsync(user.Id, user.Email);
        return TypedResults.Ok();
    }

    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation("Log out of account")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Logged out successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
    public async Task<NoContent> LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return TypedResults.NoContent();
    }

    // Helper functions
    private async Task AuthenticateUserAsync(int userId, string email)
    {
        var claims = new List<Claim> 
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email) 
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            IssuedUtc = DateTime.UtcNow,
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}
