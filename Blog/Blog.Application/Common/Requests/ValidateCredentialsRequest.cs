using FluentValidation;

namespace Blog.Application.Common.Requests;

public record ValidateCredentialsRequest(string Email, string Password);

public sealed class ValidateCredentialsRequestValidator : AbstractValidator<ValidateCredentialsRequest>
{
    public ValidateCredentialsRequestValidator()
    {
        RuleFor(e => e.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(e => e.Password)
            .NotEmpty();
    }
}