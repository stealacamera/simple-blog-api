using FluentValidation;

namespace Blog.Application.Common.Requests.PostRequests;

public record AddCategoriesToPostRequest(int[] CategoryIds);

public sealed class AddCategoriesToPostRequestValidator : AbstractValidator<AddCategoriesToPostRequest>
{
    public AddCategoriesToPostRequestValidator()
    {
        RuleFor(e => e.CategoryIds)
            .NotEmpty()
            .Must(e => e.Length <= 15).WithMessage("Cannot add more than 15 categories at once");
    }
}