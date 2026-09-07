using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Facturity.Modules.Customer.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddClientesFeatures(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CrearClienteCommand, Result<Guid>>, CrearClienteHandler>();
        services.AddScoped<IQueryHandler<ListarClientesQuery, Result<IEnumerable<Cliente>>>, ListarClientesHandler>();
        services.AddScoped<ICommandHandler<EliminarClienteCommand, Result>, EliminarClienteHandler>();
        services.AddScoped<IQueryHandler<ObtenerClientePorIdQuery, Result<ClienteDto>>, ObtenerClientePorIdHandler>();
        services.AddScoped<ICommandHandler<DesactivarClienteCommand, Result>, DesactivarClienteHandler>();
        services.AddScoped<ICommandHandler<ActivarClienteCommand, Result>, ActivarClienteHandler>();
        return services;
    }
}