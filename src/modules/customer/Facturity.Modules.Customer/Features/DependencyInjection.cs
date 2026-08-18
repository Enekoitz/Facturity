using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Facturity.Modules.Customer.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddClientesFeatures(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CrearClienteCommand, Result<Guid>>, CrearClienteHandler>();
        services.AddScoped<ListarClientesHandler>();
        // services.AddScoped<ICommandHandler<ListarClientesCommand, Result<Guid>>, ListarClientesHandler>();
        return services;
    }
}