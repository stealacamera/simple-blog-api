using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;
using Blog.Application.Common.Validation;
using Blog.Domain.Common.Enums;
using FluentValidation;

namespace Blog.Application.Common.Requests;

public record CreatePostRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }

    [JsonConverter(typeof(SmartEnumValueConverter<PostStatuses, sbyte>))]
    public required PostStatuses Status { get; init; }

    public required IList<int> CategoryIds { get; init; }
}

public sealed class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(e => e.Title)
            .MaximumLength(ValidationUtils.PostMaxTitle)
            .NotEmpty();

        RuleFor(e => e.Content)
            .MaximumLength(ValidationUtils.PostMaxContent)
            .NotEmpty();

        RuleFor(e => e.Status)
            .Must(e => PostStatuses.TryFromValue(e, out _)).WithMessage("Invalid value")
            .NotEmpty();

        RuleFor(e => e.CategoryIds)
            .NotEmpty()
            .Must(e => e.Count <= 10).WithMessage("At most 10 categories can be attached to a post");
    }
}