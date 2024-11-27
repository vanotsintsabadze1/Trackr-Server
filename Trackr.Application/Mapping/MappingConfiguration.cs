using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Mapping;

public static class MappingConfiguration
{
    public static void ConfigureMapping(this IServiceCollection services)
    {
        ConfigureTransactionMapping();
    }

    private static void ConfigureTransactionMapping()
    {
        TypeAdapterConfig<TransactionRequestModel, Transaction>.NewConfig()
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Amount, src => src.Amount);
    }
}