using Microsoft.Extensions.DependencyInjection;
using Trackr.Application.Interfaces;
using Trackr.Infrastructure.Repositories;
using Trackr.Infrastructure.Utility;

namespace Trackr.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IJwtManager, JwtManager>();
        return services;
    }
}
