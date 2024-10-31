using System.Net;
using System.Text.Json;
using Trackr.API.Infrastructure.Errors;

namespace Trackr.API.Infrastructure.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private RequestDelegate _next;
    private ILogger _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;

    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        } catch (Exception ex)
        {
            await HandleException(ex, ctx);
        }
    }

    public async Task HandleException(Exception ex, HttpContext ctx)
    {
        APIError apiError = new(ctx, ex);
        var serializedError = JsonSerializer.Serialize(apiError);

        if (apiError.Status == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogCritical("Critical error occured - {0}", ex.StackTrace);
        } else
        {
            _logger.LogError("Error occured - {0}", ex.StackTrace);
        }

        ctx.Response.Clear();
        ctx.Response.StatusCode = apiError.Status!.Value;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsync(serializedError);
    }


}
