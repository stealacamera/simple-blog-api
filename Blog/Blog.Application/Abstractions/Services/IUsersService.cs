using Blog.Application.Common.DTOs;
using Blog.Application.Common.Requests;

namespace Blog.Application.Abstractions.Services;

public interface IUsersService
{
    Task<User> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
}
