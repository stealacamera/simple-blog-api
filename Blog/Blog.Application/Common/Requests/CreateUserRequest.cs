using FluentValidation;

namespace Blog.Application.Common.Requests;

public record CreateUserRequest(string Username, string Email, string Password);

public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(e => e.Username)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(e => e.Email)
            .EmailAddress()
            .MaximumLength(150)
            .NotEmpty();

        RuleFor(e => e.Password)
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-zA-Z])(?=.*\d).*$").WithMessage("Password requires a combination of letters and numbers")
            .NotEmpty();
    }
}