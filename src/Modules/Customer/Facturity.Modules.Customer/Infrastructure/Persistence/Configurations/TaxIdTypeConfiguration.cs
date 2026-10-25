using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Infrastructure;

public class TaxIdTypeConfiguration : IEntityTypeConfiguration<TaxIdTypesCatalog>
{
    public void Configure(EntityTypeBuilder<TaxIdTypesCatalog> builder)
    {
        builder.ToTable("tax_id_types_catalog");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("smallint")
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("code");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("full_name");

        builder.Property(e => e.TicketBaiCode)
            .HasMaxLength(3)
            .HasColumnName("code_ticket_bai_bizkaia");

        builder.HasData(
            TaxIdTypesCatalog.SpanishTaxId,
            TaxIdTypesCatalog.IntraCommunityVatId,
            TaxIdTypesCatalog.Passport,
            TaxIdTypesCatalog.CountryOfOriginIdentityDocument,
            TaxIdTypesCatalog.ResidenceCertificate,
            TaxIdTypesCatalog.OtherSupportingDocument
        );
    }
}
