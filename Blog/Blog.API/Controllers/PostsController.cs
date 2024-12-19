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

    [HttpGet]
    [AllowAnonymous]
    public async Task<Ok<IList<Post>>> GetAllAsync(
        CancellationToken cancellationToken,
        string? filterByTitle = null,
        string? filterByContent = null,
        DateOnly? filterByPublicationDate = null)
    {
        var posts = await _servicesManager.PostsService
                                          .GetAllAsync(
                                                GetRequesterIdOrAnon(),
                                                filterByTitle, filterByContent, 
                                                filterByPublicationDate, cancellationToken);
        
        return TypedResults.Ok(posts);
    }

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

    [HttpPatch("{id:int:min(1)}")]
    public async Task<Ok<PostDetails>> UpdateAsync(
        int id, 
        UpdatePostRequest request,
        IValidator<UpdatePostRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var updatedPost = await _servicesManager.PostsService.UpdateAsync(id, GetRequesterId(), request, cancellationToken);
        return TypedResults.Ok(updatedPost);
    }
}
