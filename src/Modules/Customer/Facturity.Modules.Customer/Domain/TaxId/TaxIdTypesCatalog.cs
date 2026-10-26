namespace Facturity.Modules.Customer.Domain;

public sealed record TaxIdTypesCatalog
{
    public static readonly TaxIdTypesCatalog SpanishTaxId = new(TaxIdType.SpanishTaxId, "SP_TAX_ID".ToUpper(), "Spanish Tax ID", "Spanish Tax ID", "01");
    public static readonly TaxIdTypesCatalog IntraCommunityVatId = new(TaxIdType.IntraCommunityVatId, "INT_COM_VAT_ID".ToUpper(), "Intra-community VAT ID", "Intra-community VAT ID", "02");
    public static readonly TaxIdTypesCatalog Passport = new(TaxIdType.Passport, "PAS".ToUpper(), "Passport", "Passport", "03");
    public static readonly TaxIdTypesCatalog CountryOfOriginIdentityDocument = new(TaxIdType.CountryOfOriginIdentityDocument, "ORIGIN_ID_DOC".ToUpper(), "Official identification document issued by the country or territory of residence", "Official identification document issued by the country or territory of residence", "04");
    public static readonly TaxIdTypesCatalog ResidenceCertificate = new(TaxIdType.ResidenceCertificate, "RES_CERT".ToUpper(), "Residence Certificate", "Residence Certificate", "05");
    public static readonly TaxIdTypesCatalog OtherSupportingDocument = new(TaxIdType.OtherSupportingDocument, "OTH_SUPP_DOC".ToUpper(), "Other supporting document", "Other supporting document", "06");

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