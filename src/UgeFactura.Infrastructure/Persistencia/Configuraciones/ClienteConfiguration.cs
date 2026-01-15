using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Clientes;

namespace UgeFactura.Infrastructure.Persistencia.Configuraciones
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("clientes");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Alias).IsRequired().HasMaxLength(50);

            // Mapear VO como propiedades propias o tablas separadas
            builder.OwnsOne(c => c.Identificacion, id =>
            {
                id.Property(i => i.Tipo).HasColumnName("tipo_codigo").IsRequired();
                id.Property(i => i.Valor).HasColumnName("valor").IsRequired();
                id.Property(i => i.PaisCodigo).HasColumnName("pais");
            });

            builder.OwnsOne(c => c.Domicilio, d =>
            {
                d.Property(x => x.Direccion).HasColumnName("direccion").IsRequired();

                d.OwnsOne(x => x.CP, cp =>
                {
                    cp.Property(c => c.Valor)   // asumimos que VO tiene propiedad Value
                      .HasColumnName("codigo_postal")
                      .IsRequired();
                });

                d.Property(x => x.Municipio).HasColumnName("municipio");
                d.Property(x => x.Provincia).HasColumnName("provincia");
                d.Property(x => x.Region).HasColumnName("region");
                d.Property(x => x.Pais).HasColumnName("pais");
            });

            // Otros campos opcionales
            builder.Property(c => c.RazonSocial).HasMaxLength(100);
            builder.Property(c => c.Nombre).HasMaxLength(50);
            builder.Property(c => c.PrimerApellido).HasMaxLength(50);
            builder.Property(c => c.SegundoApellido).HasMaxLength(50);
        }
    }
}
