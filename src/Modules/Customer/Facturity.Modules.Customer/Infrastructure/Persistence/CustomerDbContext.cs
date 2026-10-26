using Facturity.Modules.Customer.Domain;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Customer.Infrastructure;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public DbSet<CustomerRecord> Customers { get; set; }
    public DbSet<TaxIdTypesCatalog> TaxIdTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("customer");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
