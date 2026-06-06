using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UgeFactura.Modules.Clientes.Domain;

namespace UgeFactura.Modules.Clientes.Infrastructure
{
    public class TipoIdentificacionConfiguration : IEntityTypeConfiguration<TipoIdentificacionFiscal>
    {
        public void Configure(EntityTypeBuilder<TipoIdentificacionFiscal> builder)
        {
            builder.ToTable("tipos_identificacion_fiscal");

            builder.HasKey(c => c.Codigo).HasName("codigo").HasName("id");
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(250).HasColumnName("nombre");

            builder.Property(e => e.Regla)
                .HasConversion(
                    v => v.ToString(),
                    v => IdentificacionFiscalService.ParseOrDesconocido(v)
                )
                .HasMaxLength(250)
                .IsRequired()
                .HasColumnName("regla");

            builder.Property(c => c.RequierePais).IsRequired().HasColumnName("requiere_pais");

            builder.HasData(
                new TipoIdentificacionFiscal(TipoIdentificacionFiscalEnum.NIF.ToCodigo(), "NIF-IVA", false),
                new TipoIdentificacionFiscal(TipoIdentificacionFiscalEnum.Pasaporte.ToCodigo(), "Pasaporte", false),
                new TipoIdentificacionFiscal(TipoIdentificacionFiscalEnum.DocumentoExpedidoPaisResidencia.ToCodigo(), "Documento oficial de identificación expedido por el país o territorio de residencia", false),
                new TipoIdentificacionFiscal(TipoIdentificacionFiscalEnum.CertificadoResidencia.ToCodigo(), "Certificado de residencia", false),
                new TipoIdentificacionFiscal(TipoIdentificacionFiscalEnum.OtroDocumentoProbatorio.ToCodigo(), "Otro documento probatorio", false)
            );
        }
    }
}
