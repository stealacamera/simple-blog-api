using Blog.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

public static class Startup
{
    public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddScoped<IWorkUnit, WorkUnit>();
    }
}
