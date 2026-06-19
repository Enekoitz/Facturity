namespace UgeFactura.Modules.Clientes.Domain
{
    public static class TipoIdentificacionFiscalService
    {
        public static string ToCodigo(this TipoIdentificacionFiscalEnum tipo) => tipo switch
        {
            TipoIdentificacionFiscalEnum.NIF => "02",
            TipoIdentificacionFiscalEnum.Pasaporte => "03",
            TipoIdentificacionFiscalEnum.DocumentoExpedidoPaisResidencia => "04",
            TipoIdentificacionFiscalEnum.CertificadoResidencia => "05",
            TipoIdentificacionFiscalEnum.OtroDocumentoProbatorio => "06",
            _ => throw new ArgumentOutOfRangeException(nameof(tipo))
        };
    }
}
