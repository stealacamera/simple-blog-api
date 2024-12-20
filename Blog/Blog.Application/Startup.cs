using Blog.Application.Abstractions;
using Blog.Domain.Common.Enums;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blog.Application;

public static class Startup
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        services.AddScoped<IServicesManager, ServicesManager>();

        // Register smart enums in schema
        services.Configure<SwaggerGenOptions>(e =>
            e.MapType<PostStatuses>(() =>
            {
                var enumValues = new List<IOpenApiAny>(PostStatuses.List.Select(e => new OpenApiInteger(e.Value)).ToList());
                var description = $"Post status: {string.Join(", ", PostStatuses.List.Select(e => $"{e.Name}: {e.Value}"))}";

                return new OpenApiSchema { Type = "enum", Enum = enumValues, Description = description };
            }));
    }
}
