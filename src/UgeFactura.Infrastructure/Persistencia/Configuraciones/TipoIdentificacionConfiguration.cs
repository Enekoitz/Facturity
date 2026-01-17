using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UgeFactura.Domain.Clientes;
using static UgeFactura.Domain.Clientes.Servicios.ReglaTipoIdentificacionHelper;

namespace UgeFactura.Infrastructure.Persistencia.Configuraciones
{
    public class TipoIdentificacionConfiguration : IEntityTypeConfiguration<TipoIdentificacionFiscal>
    {
        public void Configure(EntityTypeBuilder<TipoIdentificacionFiscal> builder)
        {
            builder.ToTable("tipos_identificacion_fiscal");

            builder.HasKey(c => c.Codigo).HasName("identificacion_tipo_id");
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(50).HasColumnName("nombre");

            // Mapear enum Regla a string en BD
            builder.Property(e => e.Regla)
                .HasConversion(
                    v => v.ToString(),           // Del enum a string en BD
                    v => ReglaIdentificacionFiscalHelper.ParseOrDesconocido(v)
                )
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("regla");

            builder.Property(c => c.RequierePais).IsRequired().HasColumnName("requierePais");
        }
    }
}
