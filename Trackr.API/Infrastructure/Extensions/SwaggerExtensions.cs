using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;
using Trackr.API.Infrastructure.Helpers;

namespace Trackr.API.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new ControllerNameTransformer()));
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            });

            var securityRequirements = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            };

            x.AddSecurityRequirement(securityRequirements);

        });

        return services;
    }
}
