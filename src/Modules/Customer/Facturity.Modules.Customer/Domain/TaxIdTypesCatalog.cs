namespace Facturity.Modules.Customer.Domain;

public sealed record TaxIdTypesCatalog
{
    //TODO
    public static readonly TaxIdTypesCatalog Nif = new(TaxIdType.TaxId, "TAX_ID".ToUpper(), "NIF", "Número de identificación fiscal", "01");
    public static readonly TaxIdTypesCatalog NifIvaComunitario = new(TaxIdType.IntraCommunityVatId, "INTRA_COMMUNITY_VAT_ID".ToUpper(), "NIF-IVA", "NIF-IVA Comunitario", "02");
    public static readonly TaxIdTypesCatalog Pasaporte = new(TaxIdType.Passport, "PAS".ToUpper(), "Pasaporte", "Pasaporte", "03");
    public static readonly TaxIdTypesCatalog DocumentoIdentidadPaisOrigen = new(TaxIdType.CountryOfOriginIdentityDocument, "DOC_PAI".ToUpper(), "Documento oficial de identificación expedido por el país o territorio de residencia", "Documento oficial de identificación expedido por el país o territorio de residencia", "04");
    public static readonly TaxIdTypesCatalog CertificadoResidencia = new(TaxIdType.ResidenceCertificate, "CER_RES".ToUpper(), "Certificado de residencia", "Certificado de residencia", "05");
    public static readonly TaxIdTypesCatalog OtroDocumentoProbatorio = new(TaxIdType.OtherSupportingDocument, "OTR_DOC_PRO".ToUpper(), "Otro documento probatorio", "Otro documento probatorio", "06");

    public TaxIdType Id { get; }
    public string Code { get; }
    public string Name { get; }
    public string FullName { get; }
    public string TicketBaiCode { get; }
    
    private TaxIdTypesCatalog() { }

    private TaxIdTypesCatalog(TaxIdType id, string code, string name, string fullName, string ticketBaiCode)
    {
        Id = id;
        Code = code;
        Name = name;
        FullName = fullName;
        TicketBaiCode = ticketBaiCode;
    }
}