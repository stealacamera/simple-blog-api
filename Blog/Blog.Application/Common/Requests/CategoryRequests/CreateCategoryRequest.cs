using FluentValidation;

namespace Blog.Application.Common.Requests.CategoryRequests;

public record CreateCategoryRequest(string Name, string Description);

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(e => e.Name)
            .MaximumLength(ValidationUtils.CategoryMaxName)
            .NotEmpty();

        RuleFor(e => e.Description)
            .MaximumLength(ValidationUtils.CategoryMaxDescription)
            .NotEmpty();
    }
}
