using Microsoft.Extensions.DependencyInjection;
using Trackr.Application.Interfaces;
using Trackr.Application.Services;

namespace Trackr.Application.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
