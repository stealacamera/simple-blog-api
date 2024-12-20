using Serilog;

namespace Blog.API.Common;

public static class StartupUtils
{
    public static void RegisterApiServices(this WebApplicationBuilder builder)
    {
        // Register logger
        Log.Logger = new LoggerConfiguration().ReadFrom
                                              .Configuration(builder.Configuration)
                                              .CreateLogger();

        builder.Logging.AddSerilog(Log.Logger);
        builder.Host.UseSerilog();

        // Registering services
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();
    }
}
