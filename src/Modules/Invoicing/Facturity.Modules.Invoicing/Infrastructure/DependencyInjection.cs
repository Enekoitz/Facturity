using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Facturity.Modules.Invoicing.Features;

namespace Facturity.Modules.Invoicing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInvoicingInfrastructure(this IServiceCollection services, string connectionString)
    {
        // services.AddScoped<AuditSaveChangesInterceptor>();
            
        services.AddDbContext<InvoicingDbContext>((sp, options) =>
            options.UseNpgsql(connectionString,
                o => o.MigrationsHistoryTable("__EFMigrationsHistory", "invoicing"))
                // .AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>())
            );

        services.AddScoped<IInvoicingRepository, InvoicingRepository>();
        return services;
    }
}
