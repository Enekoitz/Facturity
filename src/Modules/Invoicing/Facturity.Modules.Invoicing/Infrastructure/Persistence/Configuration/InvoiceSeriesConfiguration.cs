using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.Infrastructure;

public class InvoiceSeriesConfiguration : IEntityTypeConfiguration<InvoiceSeries>
{
    void IEntityTypeConfiguration<InvoiceSeries>.Configure(EntityTypeBuilder<InvoiceSeries> builder)
    {
        builder.ToTable("invoice_series");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedNever();
        
        builder.Property(i => i.TenantId)
            .HasColumnName("tenant_id")
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(i => i.Name)
            .HasMaxLength(InvoiceSeries.NameMaxLength)
            .IsRequired()
            .HasColumnName("name");
        
        builder.Property(i => i.Description)
            .HasMaxLength(InvoiceSeries.DescriptionMaxLength)
            .HasColumnName("description");

        // builder.Property<DateTimeOffset>("created_at_utc").HasColumnType("timestamptz").IsRequired();
        // builder.Property<DateTimeOffset?>("updated_at_utc").HasColumnType("timestamptz");
        // builder.Property<string>("created_by").HasMaxLength(20).IsRequired();
        // builder.Property<string>("updated_by").HasMaxLength(20);
    }
}
