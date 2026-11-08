using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.Infrastructure;

public class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    void IEntityTypeConfiguration<InvoiceLine>.Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.ToTable("invoice_lines");
        
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .HasColumnOrder(0)
            .ValueGeneratedOnAdd();
        
        builder.Property(i => i.TenantId)
            .HasColumnName("tenant_id")
            .HasColumnType("uuid")
            .HasColumnOrder(1)
            .ValueGeneratedOnAdd();
        
        builder.Property(i => i.InvoiceId)
            .HasColumnName("invoice_id")
            .HasColumnType("uuid")
            .HasColumnOrder(2)
            .ValueGeneratedNever();
        
        builder.Property(i => i.Description)
            .HasColumnName("description")
            .HasColumnOrder(3)
            .HasMaxLength(InvoiceSeries.DescriptionMaxLength)
            .IsRequired();

        builder.Property(i => i.Units)
            .HasColumnName("units")
            .HasColumnType($"decimal(18,{InvoiceLine.UnitsMaxDecimalPlaces})")
            .HasColumnOrder(4)
            .IsRequired();

        builder.OwnsOne(i => i.UnitPrice, u =>
        {
            u.Property(p => p.Price)
                .HasColumnName("unit_price")
                .HasColumnType($"decimal(18,{UnitPrice.MaxDecimalPlaces})")
                .HasColumnOrder(5)
                .IsRequired();
        });
        
        builder.OwnsOne(i => i.TaxAmount, u =>
        {
            u.Property(p => p.Amount)   
                .HasColumnName("tax_amount")
                .HasColumnType($"decimal(18,{Money.MaxDecimalPlaces})")
                .HasColumnOrder(6)
                .IsRequired();
        });
        
        builder.OwnsOne(i => i.TaxBase, u =>
        {
            u.Property(p => p.Amount)   
                .HasColumnName("tax_base")
                .HasColumnType($"decimal(18,{Money.MaxDecimalPlaces})")
                .HasColumnOrder(7)
                .IsRequired();
        });
        
        builder.OwnsOne(i => i.TaxRate, u =>
        {
            u.Property(p => p.Percentage)   
                .HasColumnName("tax_rate")
                .HasColumnType($"decimal(5,{TaxRate.MaxDecimalPlaces})")
                .HasColumnOrder(8)
                .IsRequired();
        });
        
        builder.OwnsOne(i => i.TotalAmount, u =>
        {
            u.Property(p => p.Amount)   
                .HasColumnName("total_amount")
                .HasColumnType($"decimal(18,{Money.MaxDecimalPlaces})")
                .HasColumnOrder(9)
                .IsRequired();
        });

        // builder.Property<DateTimeOffset>("created_at_utc").HasColumnType("timestamptz").IsRequired();
        // builder.Property<DateTimeOffset?>("updated_at_utc").HasColumnType("timestamptz");
        // builder.Property<string>("created_by").HasMaxLength(20).IsRequired();
        // builder.Property<string>("updated_by").HasMaxLength(20);
    }
}
