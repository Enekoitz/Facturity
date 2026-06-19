using Microsoft.EntityFrameworkCore;
using UgeFactura.Modules.Clientes.Domain;
namespace UgeFactura.Modules.Clientes.Infrastructure;
public class ClientesDbContext : DbContext
{
    public ClientesDbContext(DbContextOptions<ClientesDbContext> options)
        : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<TipoIdentificacionFiscal> TipoIdentificacionFiscal { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("clientes");
        modelBuilder.ApplyConfiguration(new TipoIdentificacionConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
