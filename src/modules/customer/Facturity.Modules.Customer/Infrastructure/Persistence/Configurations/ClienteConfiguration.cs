using Facturity.Modules.Customer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        builder.Property(c => c.RazonSocial).HasMaxLength(150).HasColumnName("razon_social");
        builder.Property(c => c.Nombre).HasMaxLength(50).HasColumnName("nombre");
        builder.Property(c => c.PrimerApellido).HasMaxLength(50).HasColumnName("primer_apellido");
        builder.Property(c => c.SegundoApellido).HasMaxLength(50).HasColumnName("segundo_apellido");

        builder.OwnsOne(c => c.Domicilio, d =>
        {
            d.Property(x => x.Direccion).HasMaxLength(200).HasColumnName("domicilio_direccion").IsRequired();
            d.Property(x => x.Municipio).HasMaxLength(100).HasColumnName("domicilio_municipio");
            d.Property(x => x.Provincia).HasMaxLength(50).HasColumnName("domicilio_provincia");
            d.Property(x => x.Region).HasMaxLength(50).HasColumnName("domicilio_region");
            d.Property(x => x.Pais).HasMaxLength(60).HasColumnName("domicilio_pais");

            d.OwnsOne(x => x.CodigoPostal, cp =>
            {
                cp.Property(c => c.Valor)
                    .HasMaxLength(5)
                    .HasColumnName("domicilio_codigo_postal")
                    .IsRequired();
            });
        });
        
        builder.Property(c => c.DeletedAtUtc)
            .HasColumnName("deleted_at_utc")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql(null);
    }
}
