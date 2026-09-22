using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public class TiposIdentificacionFiscalConfiguration : IEntityTypeConfiguration<TipoIdentificacionFiscal>
{
    public void Configure(EntityTypeBuilder<TipoIdentificacionFiscal> builder)
    {
        builder.ToTable("catalogo_tipos_identificacion_fiscal");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("smallint")
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Codigo)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("codigo");

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("nombre");

        builder.Property(c => c.NombreCompleto)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("nombre_completo");

        builder.Property(e => e.CodigoTicketBai)
            .HasMaxLength(3)
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
