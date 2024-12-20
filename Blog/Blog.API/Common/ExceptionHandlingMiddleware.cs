
using Blog.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Common;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly static Dictionary<Type, (int, string)> _exceptionCodes = new()
    {
        { typeof(EntityNotFoundException), (StatusCodes.Status404NotFound, "Object not found") },
        { typeof(UnauthorizedException), (StatusCodes.Status401Unauthorized, "Unauthorized access") },
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case BaseException baseEx:
                    await HandleApiErrors(baseEx, context);
                    break;
                case ValidationException validationEx:
                    await HandleValidationErrors(validationEx, context);
                    break;
                default:
                    await HandleErrors(ex, context);
                    break;
            }
        }
    }

    private async Task HandleApiErrors(BaseException ex, HttpContext context)
    {
        ProblemDetails details = new();

        if (_exceptionCodes.ContainsKey(ex.GetType()))
        {
            details.Title = _exceptionCodes[ex.GetType()].Item2;
            details.Detail = ex.Message;

            details.Status = _exceptionCodes[ex.GetType()].Item1;
        }
        else
        {
            details.Title = ex.Message;
            details.Status = StatusCodes.Status400BadRequest;
        }

        context.Response.StatusCode = details.Status.Value;
        await context.Response.WriteAsJsonAsync(details);
    }

    private async Task HandleValidationErrors(ValidationException ex, HttpContext context)
    {
        Dictionary<string, string[]> errors = new();

        foreach (var error in ex.Errors)
        {
            if (errors.ContainsKey(error.PropertyName))
                errors[error.PropertyName].Append(error.ErrorMessage);
            else
                errors.Add(error.PropertyName, [error.ErrorMessage]);
        }

        var details = new ValidationProblemDetails
        {
            Title = "Validation errors occurred",
            Errors = errors,
            Status = StatusCodes.Status400BadRequest
        };

        context.Response.StatusCode = details.Status.Value;
        await context.Response.WriteAsJsonAsync(details);
    }

    private async Task HandleErrors(Exception ex, HttpContext context)
    {
        var details = new ProblemDetails
        {
            Title = "Server error",
            Detail = "Something went wrong in the server, try again later",
            Status = StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = details.Status.Value;
        await context.Response.WriteAsJsonAsync(details);
    }
}
