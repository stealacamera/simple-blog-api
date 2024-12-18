using Blog.Application.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public static class Startup
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

        services.AddScoped<IServicesManager, ServicesManager>();
    }
}
