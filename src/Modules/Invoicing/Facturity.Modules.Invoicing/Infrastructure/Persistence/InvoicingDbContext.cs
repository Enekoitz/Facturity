using Facturity.Modules.Invoicing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Invoicing.Infrastructure;

public class InvoicingDbContext(DbContextOptions<InvoicingDbContext> options) : DbContext(options)
{
    internal DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("invoicing");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoicingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
