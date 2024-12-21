using Blog.Application.Abstractions;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.DTOs.Requests.PostRequests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : BaseController
{
    public PostsController(IServicesManager servicesManager) : base(servicesManager) { }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation("Retrieve all posts", "If requester is a user, retrieve all existing posts. Otherwise, only public posts are shown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Posts retrieved successfully", typeof(IList<Post>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
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
    [SwaggerOperation("Create a new post")]
    [SwaggerResponse(StatusCodes.Status201Created, "Post created successfully", typeof(Post))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Specified categories don't exist", typeof(ProblemDetails))]
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
    [SwaggerOperation("Update existing post")]
    [SwaggerResponse(StatusCodes.Status200OK, "Post updated successfully", typeof(PostDetails))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, type: typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Post could not be found", typeof(ProblemDetails))]
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

    [HttpDelete("{id:int:min(1)}")]
    [SwaggerOperation("Delete existing post")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Post deleted successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Requester is not logged in or is not the post's owner", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Post could not be found", typeof(ProblemDetails))]
    public async Task<NoContent> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _servicesManager.PostsService.DeleteAsync(id, GetRequesterId(), cancellationToken);
        return TypedResults.NoContent();
    }

    [HttpPost("{id:int:min(1)}/categories")]
    [SwaggerOperation("Add categories to a post", "If any of the specified categories don't exist or are already linked to post, they are skipped")]
    [SwaggerResponse(StatusCodes.Status200OK, "Post updated successfully", typeof(Post))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Requester is not logged in or is not the post's owner", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Post could not be found", typeof(ProblemDetails))]
    public async Task<Ok<Post>> AddCategoriesAsync(
        int id,
        UpdateCategoriesForPostRequest request,
        IValidator<UpdateCategoriesForPostRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var updatedPost = await _servicesManager.PostsService
                                                .AddCategoriesToPostAsync(id, GetRequesterId(), request, cancellationToken);
        
        return TypedResults.Ok(updatedPost);
    }

    [HttpDelete("{id:int:min(1)}/categories")]
    [SwaggerOperation("Remove categories from a post", "If any of the specified categories don't exist or are not linked to post, they are skipped")]
    [SwaggerResponse(StatusCodes.Status200OK, "Post updated successfully", typeof(Post))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Requester is not logged in or is not the post's owner", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Post could not be found", typeof(ProblemDetails))]
    public async Task<Ok<Post>> RemoveCategoriesAsync(
        int id,
        UpdateCategoriesForPostRequest request,
        IValidator<UpdateCategoriesForPostRequest> validator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var updatedPost = await _servicesManager.PostsService
                                                .RemoveCategoriesFromPostAsync(id, GetRequesterId(), request, cancellationToken);

        return TypedResults.Ok(updatedPost);
    }
}
