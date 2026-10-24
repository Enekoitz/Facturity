using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public class TiposIdentificacionFiscalConfiguration : IEntityTypeConfiguration<TaxIdTypesCatalog>
{
    public void Configure(EntityTypeBuilder<TaxIdTypesCatalog> builder)
    {
        builder.ToTable("catalogo_tipos_identificacion_fiscal");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("smallint")
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("codigo");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("nombre");

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("nombre_completo");

        builder.Property(e => e.TicketBaiCode)
            .HasMaxLength(3)
            .HasColumnName("codigo_ticketbai_bizkaia");

        builder.HasData(
            TaxIdTypesCatalog.Nif,
            TaxIdTypesCatalog.NifIvaComunitario,
            TaxIdTypesCatalog.Pasaporte,
            TaxIdTypesCatalog.DocumentoIdentidadPaisOrigen,
            TaxIdTypesCatalog.CertificadoResidencia,
            TaxIdTypesCatalog.OtroDocumentoProbatorio
        );
    }
}
