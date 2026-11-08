using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.Infrastructure;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    void IEntityTypeConfiguration<Invoice>.Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .HasColumnOrder(0)
            .IsRequired()
            .ValueGeneratedNever();
        
        builder.Property(i => i.TenantId)
            .HasColumnName("tenant_id")
            .HasColumnType("uuid")
            .HasColumnOrder(1)
            .IsRequired()
            .ValueGeneratedNever();
        
        builder.Property(i => i.CustomerId)
            .HasColumnName("customer_id")
            .HasColumnType("uuid")
            .HasColumnOrder(2)
            .ValueGeneratedNever();
        
        builder.Property(i => i.SeriesId)
            .HasColumnName("series_id")
            .HasColumnType("uuid")
            .HasColumnOrder(3)
            .ValueGeneratedNever();
        
        builder.Property(i => i.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasColumnOrder(4)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(i => i.IsSimplified)
            .HasColumnName("is_simplified")
            .HasColumnType("boolean")
            .IsRequired()
            .HasColumnOrder(5);
        
        builder.OwnsOne(i => i.IssuanceInfo, d =>
        {
            d.Property(x => x.Number)
                .HasColumnName("number")
                .HasColumnType("bigint")
                .HasColumnOrder(6);
        
            d.Property(x => x.IssuedAtUtc)
                .HasColumnName("issued_at_utc")
                .HasColumnType("timestamptz")
                .HasColumnOrder(7);
        });
        
        builder.Property(i => i.TransactionAtUtc)
            .HasColumnName("transaction_at_utc")
            .HasColumnType("timestamptz")
            .HasColumnOrder(8);
        
        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasColumnOrder(9)
            .HasMaxLength(Invoice.DescriptionMaxLength);
        
        builder.Property(i => i.Currency)
            .HasColumnName("currency")
            .HasColumnOrder(10)
            .IsRequired()
            .HasMaxLength(Invoice.CurrencyExactLength);
        
                
        builder.HasMany(i => i.Lines)
            .WithOne()
            .HasForeignKey(l => l.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Navigation to InvoiceSeries has been deliberately omitted: the aggregates and InvoiceSeries are decoupled
        // and could be moved to a different context. If the same JOIN is repeated in several places,
        // it may be worth reconsidering it.
        // (adding it does not require a migration).
        builder.HasOne<InvoiceSeries>()
            .WithMany()
            .HasForeignKey(i => i.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // builder.Property<DateTimeOffset>("created_at_utc").HasColumnType("timestamptz").IsRequired();
        // builder.Property<DateTimeOffset?>("updated_at_utc").HasColumnType("timestamptz");
        // builder.Property<string>("created_by").HasMaxLength(20).IsRequired();
        // builder.Property<string>("updated_by").HasMaxLength(20);
    }
}
