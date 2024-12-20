using Blog.Application.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ReturnUnauthorized,
                    OnRedirectToLogout = ReturnUnauthorized,
                    OnRedirectToReturnUrl = ReturnUnauthorized,
                });

        services.AddScoped<IWorkUnit, WorkUnit>();
    }

    private static Task ReturnUnauthorized(RedirectContext<CookieAuthenticationOptions> context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    } 
}
