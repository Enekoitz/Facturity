using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Customer.Infrastructure.Persistence;

public class ClientesDbContext : DbContext
{
    public ClientesDbContext(DbContextOptions<ClientesDbContext> options)
        : base(options) { }

    public DbSet<ClienteRecord> Clientes { get; set; }
    //public DbSet<TipoIdentificacionFiscal> TipoIdentificacionFiscal { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("clientes");
        //modelBuilder.ApplyConfiguration(new TipoIdentificacionConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
