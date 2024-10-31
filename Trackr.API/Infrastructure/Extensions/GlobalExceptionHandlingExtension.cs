using Trackr.API.Infrastructure.Middlewares;
namespace Trackr.API.Infrastructure.Extensions;

public static class GlobalExceptionHandlingExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        return app;
    }
}
