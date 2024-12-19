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
public class PostsController : BaseController
{
    public PostsController(IServicesManager servicesManager) : base(servicesManager) { }

    [HttpPost]
    public async Task<Created<Post>> CreateAsync(
        CreatePostRequest request, 
        IValidator<CreatePostRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var newPost = await _servicesManager.PostsService.CreateAsync(GetRequesterId(), request, cancellationToken);
        return TypedResults.Created(string.Empty, newPost);
    }
}
