using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Customer.Features;

namespace Facturity.Modules.Customer.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddClientesInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
            
        services.AddDbContext<ClientesDbContext>((sp, options) =>
            options.UseNpgsql(connectionString,
                o => o.MigrationsHistoryTable("__EFMigrationsHistory", "clientes"))
                .AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>()));

        services.AddScoped<IClientesRepository, ClientesRepository>();
        return services;
    }
}
