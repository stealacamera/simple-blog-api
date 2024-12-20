using Blog.Application.Abstractions;
using Blog.Application.Abstractions.Services;
using Blog.Application.Common.DTOs;
using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Requests;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Services;

internal sealed class UsersService : BaseService, IUsersService
{
    private readonly PasswordHasher<Domain.Entities.User> _passwordHasher;

    public UsersService(IWorkUnit workUnit, PasswordHasher<Domain.Entities.User> passwordHasher) : base(workUnit)
        => _passwordHasher = passwordHasher;

    public async Task<User> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // Validate email & username aren't taken
        if (await _workUnit.UsersRepository.DoesUsernameExistAsync(request.Username, cancellationToken))
            throw new ValidationException([new ValidationFailure(nameof(request.Username), "Username is taken")]);
        else if (await _workUnit.UsersRepository.DoesEmailExistAsync(request.Email, cancellationToken))
            throw new ValidationException([new ValidationFailure(nameof(request.Email), "Email is in use by a different account")]);

        // Create new user
        var user = new Domain.Entities.User
        {
            CreatedAt = DateTime.UtcNow,
            Email = request.Email,
            Username = request.Username,
        };

        user.Password = _passwordHasher.HashPassword(user, request.Password);

        await _workUnit.UsersRepository.AddAsync(user, cancellationToken);
        await _workUnit.SaveChangesAsync();

        return new User(user.Id, user.Username, user.Email);
    }

    public async Task<User> ValidateCredentialsAsync(ValidateCredentialsRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _workUnit.UsersRepository
                                  .GetByEmailAsync(request.Email, cancellationToken);

        if (user != null)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (verificationResult == PasswordVerificationResult.Success)
                return new User(user.Id, user.Username, user.Email);
        }

        throw new WrongCredentialsExceptions();
    }
}
