using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UgeFactura.Modules.Clientes.Features;

namespace UgeFactura.Modules.Clientes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClientesInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ClientesDbContext>(options =>
                options.UseNpgsql(connectionString,
                    o => o.MigrationsHistoryTable("__EFMigrationsHistory", "clientes")));

            services.AddScoped<IClientesRepository, ClientesRepository>();
            return services;
        }
    }
}
