using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Customer.Features;

namespace Facturity.Modules.Customer.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddCustomerInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
            
        services.AddDbContext<CustomerDbContext>((sp, options) =>
            options.UseNpgsql(connectionString,
                o => o.MigrationsHistoryTable("__EFMigrationsHistory", "customer"))
                .AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>()));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}
