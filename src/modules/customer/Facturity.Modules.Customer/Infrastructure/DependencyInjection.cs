using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Customer.Features;

namespace Facturity.Modules.Customer.Infrastructure;
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
