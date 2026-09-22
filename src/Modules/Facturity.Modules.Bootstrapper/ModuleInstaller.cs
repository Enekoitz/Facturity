using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.Customer.Infrastructure;
using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Bootstrapper;

public static class ModuleInstaller
{
    //public static IServiceCollection AddAllModules(this IServiceCollection services, string connString)
    //{
    //    // Domain
    //    services.AddScoped<IIdentificacionFiscalFactory, IdentificacionFiscalFactory>();

    //    services.AddClientesFeatures();
    //    services.AddClientesInfrastructure(connString);

    //    return services;
    //}

    public static IServiceCollection AddCustomerModuleServices(this IServiceCollection services, string connString)
    {
        // Domain
        services.AddScoped<IIdentificacionFiscalFactory, IdentificacionFiscalFactory>();
        // Features
        services.AddClientesFeatures();
        // Infrastructure
        services.AddClientesInfrastructure(connString);

        return services;
    }
}
