using Facturity.Modules.Customer.Domain;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClientesDbContext(DbContextOptions<ClientesDbContext> options) : DbContext(options)
{
    public DbSet<CustomerRecord> Clientes { get; set; }
    public DbSet<TaxIdTypesCatalog> TipoIdentificacionFiscal { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("customers");
        
        modelBuilder.ApplyConfiguration(new TiposIdentificacionFiscalConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesDbContext).Assembly);
        
        
        base.OnModelCreating(modelBuilder);
    }
}
