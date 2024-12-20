using Ardalis.SmartEnum.SystemTextJson;
using System.Text.Json.Serialization;
using Blog.Domain.Common.Enums;
using FluentValidation;

namespace Blog.Application.Common.Requests.PostRequests;

public record UpdatePostRequest
{
    public string? Title { get; init; }
    public string? Content { get; init; }

    [JsonConverter(typeof(SmartEnumValueConverter<PostStatuses, sbyte>))]
    public PostStatuses? Status { get; init; }
}

public sealed class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        RuleFor(e => e.Title)
            .MaximumLength(ValidationUtils.PostMaxTitle);

        RuleFor(e => e.Content)
            .MaximumLength(ValidationUtils.PostMaxContent);

        RuleFor(e => new { e.Title, e.Content, e.Status })
            .Must(e => !(e.Title == null && e.Content == null && e.Status == null))
            .WithMessage("At least one attribute should be updated");
    }
}