using Microsoft.Extensions.DependencyInjection;

namespace Facturity.Modules.Invoicing.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddClientesFeatures(this IServiceCollection services)
    {
        // Add here use-cases injection, ex.:
        // services.AddScoped<IQueryHandler<ListarClientesQuery, Result<IEnumerable<Cliente>>>, ListarClientesHandler>();
        // services.AddScoped<ICommandHandler<EliminarClienteCommand, Result>, EliminarClienteHandler>();
        return services;
    }
}