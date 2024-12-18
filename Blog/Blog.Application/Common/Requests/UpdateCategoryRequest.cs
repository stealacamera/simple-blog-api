using FluentValidation;

namespace Blog.Application.Common.Requests;

public record UpdateCategoryRequest(string? Name = null, string? Description = null);

public sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(e => e.Name)
            .MaximumLength(ValidationUtils.CategoryMaxName);

        RuleFor(e => e.Description)
            .MaximumLength(ValidationUtils.CategoryMaxDescription);

        RuleFor(e => new { e.Name, e.Description })
            .Must(e => !string.IsNullOrWhiteSpace(e.Name) || !string.IsNullOrWhiteSpace(e.Description))
            .WithMessage("At least one property should be updated");
    }
}