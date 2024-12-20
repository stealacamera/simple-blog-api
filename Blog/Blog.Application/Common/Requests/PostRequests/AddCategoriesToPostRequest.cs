using FluentValidation;

namespace Blog.Application.Common.Requests.PostRequests;

public record UpdateCategoriesForPostRequest(int[] CategoryIds);

public sealed class AddCategoriesToPostRequestValidator : AbstractValidator<UpdateCategoriesForPostRequest>
{
    public AddCategoriesToPostRequestValidator()
    {
        RuleFor(e => e.CategoryIds)
            .NotEmpty()
            .Must(e => e.Length <= 15).WithMessage("Cannot add or remove more than 15 categories at once");
    }
}