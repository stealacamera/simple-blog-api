using Blog.Application.Common.DTOs;
using Blog.Application.Common.Requests;

namespace Blog.Application.Abstractions.Services;

public interface IPostsService
{
    Task<Post> CreateAsync(int requesterId, CreatePostRequest request, CancellationToken cancellationToken = default);
}
