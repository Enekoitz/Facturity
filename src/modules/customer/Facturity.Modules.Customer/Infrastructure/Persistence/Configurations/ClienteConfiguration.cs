using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Infra;

namespace Facturity.Modules.Customer.Infrastructure;

public class ClienteConfiguration : IEntityTypeConfiguration<ClienteRecord>
{
    public void Configure(EntityTypeBuilder<ClienteRecord> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Alias)
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnName("alias");

        builder.Property(c => c.IdentificacionFiscalTipo)
            .HasColumnType("smallint")
            .IsRequired()
            .HasColumnName("identificacion_fiscal_tipo");

        builder.HasOne<TipoIdentificacionFiscal>()
            .WithMany()
            .HasForeignKey(c => (int)c.IdentificacionFiscalTipo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.IdentificacionFiscalValor)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("identificacion_fiscal_valor");

        builder.Property(c => c.IdentificacionFiscalCodigoPais)
            .IsRequired(false)
            .HasMaxLength(2)
            .HasColumnName("identificacion_fiscal_codigo_pais");
        
        const string colNameRazonSocial = "razon_social";
        const string colNameNombre = "nombre";
        const string colNamePrimerApellido = "primer_apellido";
        const string colNameSegundoApellido = "segundo_apellido";

        builder.Property(c => c.RazonSocial).HasMaxLength(150).HasColumnName(colNameRazonSocial);
        builder.Property(c => c.Nombre).HasMaxLength(50).HasColumnName(colNameNombre);
        builder.Property(c => c.PrimerApellido).HasMaxLength(50).HasColumnName(colNamePrimerApellido);
        builder.Property(c => c.SegundoApellido).HasMaxLength(50).HasColumnName(colNameSegundoApellido);

        const string colNameDireccion = "domicilio_direccion";
        const string colNameMunicipio = "domicilio_municipio";
        const string colNameProvincia = "domicilio_provincia";
        const string colNameRegion = "domicilio_region";
        const string colNamePais = "domicilio_pais";
        const string colNameCodigoPostal = "domicilio_codigo_postal";
        
        builder.OwnsOne(c => c.Domicilio, d =>
        {
            d.Property(x => x.Direccion).HasMaxLength(200).HasColumnName(colNameDireccion).IsRequired();
            d.Property(x => x.Municipio).HasMaxLength(100).HasColumnName(colNameMunicipio);
            d.Property(x => x.Provincia).HasMaxLength(50).HasColumnName(colNameProvincia);
            d.Property(x => x.Region).HasMaxLength(50).HasColumnName(colNameRegion);
            d.Property(x => x.Pais).HasMaxLength(60).HasColumnName(colNamePais);

            d.OwnsOne(x => x.CodigoPostal, cp =>
            {
                cp.Property(c => c.Valor)
                    .HasMaxLength(5)
                    .HasColumnName(colNameCodigoPostal)
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

        builder.Property(c => c.DireccionBusqueda)
            .HasColumnName("direccion_busqueda")
            .HasComputedColumnSql(
                DbUtilities.BuildPostgresConcatExpression(", ",colNameDireccion,  colNameMunicipio, 
                    colNameCodigoPostal, colNameProvincia, colNameRegion, colNamePais),
                stored: true)
            .ValueGeneratedOnAddOrUpdate();
        
        builder.Property(c => c.DenominacionFiscalBusqueda)
            .HasColumnName("denominacion_fiscal_busqueda")
            .HasComputedColumnSql(
                $"CASE WHEN {colNameRazonSocial} IS NOT NULL THEN {colNameRazonSocial} "
                + $"ELSE TRIM({DbUtilities.BuildPostgresConcatExpression(" ", 
                    colNameNombre, colNamePrimerApellido, colNameSegundoApellido)}) END",
                stored: true)
            .ValueGeneratedOnAddOrUpdate();  
    }
}
