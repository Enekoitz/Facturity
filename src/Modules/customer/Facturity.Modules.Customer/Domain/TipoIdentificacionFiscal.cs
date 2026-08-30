namespace Facturity.Modules.Customer.Domain;

public sealed record TipoIdentificacionFiscal
{
    public static readonly TipoIdentificacionFiscal Nif = new(TiposIdentificacionFiscal.Nif, "NIF".ToUpper(), "NIF", "Número de identificación fiscal", "01");
    public static readonly TipoIdentificacionFiscal NifIvaComunitario = new(TiposIdentificacionFiscal.NifIvaComunitario, "NIF_IVA".ToUpper(), "NIF-IVA", "NIF-IVA Comunitario", "02");
    public static readonly TipoIdentificacionFiscal Pasaporte = new(TiposIdentificacionFiscal.Pasaporte, "PAS".ToUpper(), "Pasaporte", "Pasaporte", "03");
    public static readonly TipoIdentificacionFiscal DocumentoIdentidadPaisOrigen = new(TiposIdentificacionFiscal.DocumentoIdentidadPaisOrigen, "DOC_PAI".ToUpper(), "Documento oficial de identificación expedido por el país o territorio de residencia", "Documento oficial de identificación expedido por el país o territorio de residencia", "04");
    public static readonly TipoIdentificacionFiscal CertificadoResidencia = new(TiposIdentificacionFiscal.CertificadoResidencia, "CER_RES".ToUpper(), "Certificado de residencia", "Certificado de residencia", "05");
    public static readonly TipoIdentificacionFiscal OtroDocumentoProbatorio = new(TiposIdentificacionFiscal.OtroDocumentoProbatorio, "OTR_DOC_PRO".ToUpper(), "Otro documento probatorio", "Otro documento probatorio", "06");

    public TiposIdentificacionFiscal Id { get; }
    public string Codigo { get; }
    public string Nombre { get; }
    public string NombreCompleto { get; }
    public string CodigoTicketBai { get; }
    
    private TipoIdentificacionFiscal() { }

    private TipoIdentificacionFiscal(TiposIdentificacionFiscal id, string codigo, string nombre, string nombreCompleto, string codigoTicketBai)
    {
        Id = id;
        Codigo = codigo;
        Nombre = nombre;
        NombreCompleto = nombreCompleto;
        CodigoTicketBai = codigoTicketBai;
    }
}