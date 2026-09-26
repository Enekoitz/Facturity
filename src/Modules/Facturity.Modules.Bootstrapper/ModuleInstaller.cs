using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.Customer.Infrastructure;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Auth.Infrastructure;

namespace Facturity.Modules.Bootstrapper;

public static class ModuleInstaller
{
    extension(IServiceCollection services)
    {
        public void AddAllModules(string connString)
        {
            services.AddAuthModuleServices(connString);
            services.AddCustomerModuleServices(connString);
        }

        private void AddAuthModuleServices(string connString)
        {
            services.AddAuthModule(connString);
        }
        
        private void AddCustomerModuleServices(string connString)
        {
            services.AddScoped<IIdentificacionFiscalFactory, IdentificacionFiscalFactory>();
            services.AddClientesFeatures();
            services.AddClientesInfrastructure(connString);

        }
    }
}
