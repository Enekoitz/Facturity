using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UgeFactura.Modules.Clientes.Features;
using UgeFactura.Modules.Clientes.Infrastructure;

namespace UgeFactura.Modules.Bootstrapper
{
    public static class ModuleInstaller
    {
        public static IServiceCollection AddAllModules(this IServiceCollection services, string connString)
        {
            // Registramos el módulo de clientes
            services.AddClientesInfrastructure(connString);
            services.AddClientesFeatures();

            return services;
        }
    }
}
