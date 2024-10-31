using Asp.Versioning;

namespace Trackr.API.Infrastructure.Extensions;

public static class VersioningExtension
{
    public static IServiceCollection ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.DefaultApiVersion = ApiVersion.Default;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }

    public static WebApplication UseApiVersioning(this WebApplication app)
    {
        app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();
        return app;
    }
}
