using Serilog;
namespace Trackr.API.Infrastructure.Extensions;

public static class LoggingExtension
{
    public static IServiceCollection ConfigureLogger(this IServiceCollection services)
    {
        string fileName = "logs/logs.txt";
        string path = Path.Combine(AppContext.BaseDirectory, fileName); 

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(path)
            .CreateLogger();

        return services;
    }
}
