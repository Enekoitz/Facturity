using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Clientes;
using UgeFactura.Infrastructure.Persistencia.Configuraciones;

namespace UgeFactura.Infrastructure.Persistencia
{
    public class UgeFacturaDbContext : DbContext
    {
        public UgeFacturaDbContext(DbContextOptions<UgeFacturaDbContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
