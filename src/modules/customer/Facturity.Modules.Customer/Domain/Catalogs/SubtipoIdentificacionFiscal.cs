namespace Facturity.Modules.Customer.Domain;

public class SubtipoIdentificacionFiscal
{
    private enum SubtiposIdentificacionFiscal
    {
        Nif = 1,
        NifIvaComunitario = 2,
        Pasaporte = 3,
        DocumentoIdentidadPaisOrigen = 4,
        CertificadoResidencia = 5,
        OtroDocumentoProbatorio = 6
        //NifEntidadExtranjera = 7
    }

    public static readonly SubtipoIdentificacionFiscal Nif = new((int)SubtiposIdentificacionFiscal.Nif, "NIF".ToUpper(), "NIF", "Número de identificación fiscal", "01");
    public static readonly SubtipoIdentificacionFiscal NifIvaComunitario = new((int)SubtiposIdentificacionFiscal.NifIvaComunitario, "NIF_IVA".ToUpper(), "NIF-IVA", "NIF-IVA Comunitario", "02");
    public static readonly SubtipoIdentificacionFiscal Pasaporte = new((int)SubtiposIdentificacionFiscal.Pasaporte, "PAS".ToUpper(), "Pasaporte", "Pasaporte", "03");
    public static readonly SubtipoIdentificacionFiscal DocumentoIdentidadPaisOrigen = new((int)SubtiposIdentificacionFiscal.DocumentoIdentidadPaisOrigen, "DOC_PAI".ToUpper(), "Documento oficial de identificación expedido por el país o territorio de residencia", "Documento oficial de identificación expedido por el país o territorio de residencia", "04");
    public static readonly SubtipoIdentificacionFiscal CertificadoResidencia = new((int)SubtiposIdentificacionFiscal.CertificadoResidencia, "CER_RES".ToUpper(), "Certificado de residencia", "Certificado de residencia", "05");
    public static readonly SubtipoIdentificacionFiscal OtroDocumentoProbatorio = new((int)SubtiposIdentificacionFiscal.OtroDocumentoProbatorio, "OTR_DOC_PRO".ToUpper(), "Otro documento probatorio", "Otro documento probatorio", "06");
    //public static readonly SubtipoIdentificacionFiscal NifEntidadExtranjera = new((int)SubtiposIdentificacionFiscal.NifEntidadExtranjera, "NIF_ENT_EXT".ToUpper(), "NIF entidad extranjera", "Número de identificación fiscal de entidad extranjera", "07");

    public int Id { get; }
    public string Codigo { get; }
    public string Nombre { get; }
    public string NombreCompleto { get; }
    public string CodigoTicketBai { get; }

    private SubtipoIdentificacionFiscal(int id, string codigo, string nombre, string nombreCompleto, string codigoTicketBai)
    {
        Id = id;
        Codigo = codigo;
        Nombre = nombre;
        NombreCompleto = nombreCompleto;
        CodigoTicketBai = codigoTicketBai;
    }
}