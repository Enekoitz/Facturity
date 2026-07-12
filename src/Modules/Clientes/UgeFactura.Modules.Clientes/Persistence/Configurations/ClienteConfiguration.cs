using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UgeFactura.Modules.Clientes.Domain;
namespace UgeFactura.Modules.Clientes.Infrastructure;
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Alias)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("alias");

        builder.OwnsOne(c => c.Identificacion, id =>
        {
            id.Property(i => i.Valor)
                .HasColumnName("identificacion_fiscal_valor")
                .IsRequired();

            id.Property(i => i.CodigoPais)
                .HasColumnName("identificacion_fiscal_pais")
                .HasMaxLength(2);

            id.HasOne(i => i.Tipo)
                .WithMany()
                .HasForeignKey("identificacion_fiscal_tipo_id") 
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_cliente_tipo_identificacion");
            });

        builder.Property(c => c.RazonSocial).HasMaxLength(100).HasColumnName("razon_social");
        builder.Property(c => c.Nombre).HasMaxLength(50).HasColumnName("nombre");
        builder.Property(c => c.PrimerApellido).HasMaxLength(50).HasColumnName("primer_apellido");
        builder.Property(c => c.SegundoApellido).HasMaxLength(50).HasColumnName("segundo_apellido");

        builder.OwnsOne(c => c.Domicilio, d =>
        {
            d.Property(x => x.Direccion).HasColumnName("domicilio_direccion").IsRequired();
            d.Property(x => x.Municipio).HasColumnName("domicilio_municipio");
            d.Property(x => x.Provincia).HasColumnName("domicilio_provincia");
            d.Property(x => x.Region).HasColumnName("domicilio_region");
            d.Property(x => x.Pais).HasColumnName("domicilio_pais");

            d.OwnsOne(x => x.CodigoPostal, cp =>
            {
                cp.Property(c => c.Valor)
                    .HasColumnName("domicilio_codigo_postal")
                    .IsRequired();
            });
        });

    }
}
