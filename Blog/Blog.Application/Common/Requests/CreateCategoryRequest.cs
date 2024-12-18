using FluentValidation;

namespace Blog.Application.Common.Requests;

public record CreateCategoryRequest(string Name, string Description);

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(e => e.Name)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(e => e.Description)
            .MaximumLength(250)
            .NotEmpty();
    }
}
