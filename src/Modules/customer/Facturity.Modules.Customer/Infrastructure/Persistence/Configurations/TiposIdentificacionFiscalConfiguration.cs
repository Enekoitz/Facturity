using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public class TiposIdentificacionFiscalConfiguration : IEntityTypeConfiguration<TipoIdentificacionFiscal>
{
    public void Configure(EntityTypeBuilder<TipoIdentificacionFiscal> builder)
    {
        builder.ToTable("catalogo_tipos_identificacion_fiscal");

        builder.HasKey(c => c.Id).HasName("id");

        builder.HasKey(c => c.Codigo);
        builder.Property(c => c.Codigo)
            .ValueGeneratedNever()
            .IsRequired()
            .HasColumnName("codigo");

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(250)
            .HasColumnName("nombre");

        builder.Property(c => c.NombreCompleto)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("nombre_completo");

        builder.Property(e => e.CodigoTicketBai)
            .HasMaxLength(3)
            .IsRequired()
            .HasColumnName("codigo_ticketbai_bizkaia");

        builder.HasData(
            TipoIdentificacionFiscal.Nif,
            TipoIdentificacionFiscal.NifIvaComunitario,
            TipoIdentificacionFiscal.Pasaporte,
            TipoIdentificacionFiscal.DocumentoIdentidadPaisOrigen,
            TipoIdentificacionFiscal.CertificadoResidencia,
            TipoIdentificacionFiscal.OtroDocumentoProbatorio
        );
    }
}
