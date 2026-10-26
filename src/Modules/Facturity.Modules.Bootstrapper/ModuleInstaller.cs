using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.Customer.Features;
using Facturity.Modules.Customer.Infrastructure;
using Facturity.Modules.Auth.Features;
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
            services.AddAuthFeatures();
            services.AddAuthInfrastructure(connString);
        }
        
        private void AddCustomerModuleServices(string connString)
        {
            services.AddScoped<ITaxIdFactory, TaxIdFactory>();
            services.AddCustomerFeatures();
            services.AddCustomerInfrastructure(connString);

        }
    }
}
