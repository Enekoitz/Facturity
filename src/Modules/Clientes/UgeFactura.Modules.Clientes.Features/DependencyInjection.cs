using Microsoft.Extensions.DependencyInjection;
using UgeFactura.Modules.Clientes.Features;

namespace UgeFactura.Modules.Clientes.Features
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClientesFeatures(this IServiceCollection services)
        {
            services.AddScoped<CrearClienteHandler>();
            services.AddScoped<ObtenerClientesHandler>();
            return services;
        }
    }
}
