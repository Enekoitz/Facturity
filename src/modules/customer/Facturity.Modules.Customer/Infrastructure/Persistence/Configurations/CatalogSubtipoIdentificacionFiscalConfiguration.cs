using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public class CatalogSubtipoIdentificacionFiscalConfiguration : IEntityTypeConfiguration<SubtipoIdentificacionFiscal>
{
    public void Configure(EntityTypeBuilder<SubtipoIdentificacionFiscal> builder)
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
            SubtipoIdentificacionFiscal.Nif,
            SubtipoIdentificacionFiscal.NifIvaComunitario,
            SubtipoIdentificacionFiscal.Pasaporte,
            SubtipoIdentificacionFiscal.DocumentoIdentidadPaisOrigen,
            SubtipoIdentificacionFiscal.CertificadoResidencia,
            SubtipoIdentificacionFiscal.OtroDocumentoProbatorio
        );
    }
}
