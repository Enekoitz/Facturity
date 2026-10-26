using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Infra;

namespace Facturity.Modules.Customer.Infrastructure;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerRecord>
{
    public void Configure(EntityTypeBuilder<CustomerRecord> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Alias)
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnName("alias");

        builder.Property(c => c.TaxIdType)
            .HasColumnType("smallint")
            .IsRequired()
            .HasColumnName("tax_id_type");

        builder.HasOne<TaxIdTypesCatalog>()
            .WithMany()
            .HasForeignKey(c => (int)c.TaxIdType)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.TaxIdValue)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("tax_id_value");

        builder.Property(c => c.TaxIdCountryCode)
            .IsRequired(false)
            .HasMaxLength(2)
            .HasColumnName("tax_id_country_code");
        
        const string colNameRazonSocial = "legal_entity_name";
        const string colNameNombre = "first_name";
        const string colNamePrimerApellido = "first_surname";
        const string colNameSegundoApellido = "second_surname";

        builder.Property(c => c.LegalEntityName).HasMaxLength(150).HasColumnName(colNameRazonSocial);
        builder.Property(c => c.Name).HasMaxLength(50).HasColumnName(colNameNombre);
        builder.Property(c => c.FirstSurname).HasMaxLength(50).HasColumnName(colNamePrimerApellido);
        builder.Property(c => c.SecondSurname).HasMaxLength(50).HasColumnName(colNameSegundoApellido);

        const string addressColumnName = "address_street";
        const string cityColumnName = "address_city";
        const string stateColumnName = "address_state";
        const string regionColumnName = "address_region";
        const string countryColumnName = "address_country";
        const string postalCodeColumnName = "address_postal_code";
        
        builder.OwnsOne(c => c.Address, d =>
        {
            d.Property(x => x.Street).HasMaxLength(200).HasColumnName(addressColumnName).IsRequired();
            d.Property(x => x.City).HasMaxLength(100).HasColumnName(cityColumnName);
            d.Property(x => x.State).HasMaxLength(50).HasColumnName(stateColumnName);
            d.Property(x => x.Region).HasMaxLength(50).HasColumnName(regionColumnName);
            d.Property(x => x.Country).HasMaxLength(60).HasColumnName(countryColumnName);

            d.OwnsOne(x => x.PostalCode, cp =>
            {
                cp.Property(c => c.Valor)
                    .HasMaxLength(5)
                    .HasColumnName(postalCodeColumnName)
                    .IsRequired();
            });
        });
        
        builder.Property(c => c.ActivatedAtUtc)
            .HasColumnName("activated_at_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql(null);
        
        builder.Property(c => c.DeactivatedAtUtc)
            .HasColumnName("deactivated_at_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql(null);
        
        builder.Property(c => c.DeletedAtUtc)
            .HasColumnName("deleted_at_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql(null);

        builder.Property(c => c.AddressSearch)
            .HasColumnName("address_search")
            .HasComputedColumnSql(
                DbUtilities.BuildPostgresConcatExpression(", ",addressColumnName,  cityColumnName, 
                    postalCodeColumnName, stateColumnName, regionColumnName, countryColumnName),
                stored: true)
            .ValueGeneratedOnAddOrUpdate();
        
        builder.Property(c => c.TaxNameSearch)
            .HasColumnName("tax_name_search")
            .HasComputedColumnSql(
                $"CASE WHEN {colNameRazonSocial} IS NOT NULL THEN {colNameRazonSocial} "
                + $"ELSE TRIM({DbUtilities.BuildPostgresConcatExpression(" ", 
                    colNameNombre, colNamePrimerApellido, colNameSegundoApellido)}) END",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();  
        
        builder.Property<DateTimeOffset>("created_at_utc").HasColumnType("timestamptz").IsRequired();
        builder.Property<DateTimeOffset?>("updated_at_utc").HasColumnType("timestamptz");
        builder.Property<string>("created_by").HasMaxLength(20).IsRequired();
        builder.Property<string>("updated_by").HasMaxLength(20);
    }
}
