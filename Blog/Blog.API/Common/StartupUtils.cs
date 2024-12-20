namespace Blog.API.Common;

public static class StartupUtils
{
    public static void RegisterApiServices(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }
}
