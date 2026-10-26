using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record OtherTaxId : ITaxId
{
    public string Value { get; }
    public TaxIdType Type { get; }
    public string CountryCode { get; }
    public bool? IsLegalEntity { get; } = null;

    private OtherTaxId() { }

    private OtherTaxId(string value, TaxIdType type, string countryCode)
    {
        Value = value;
        Type = type;
        CountryCode = countryCode;
    }

    public static Result<OtherTaxId> Create(string value, TaxIdType type, string countryCode)
    {
        //TODO: Refactorize getting the Error type directly from the validation method, and centralize all errors from validation
        if (!TaxIdValidations.IsValidOtherTaxId(countryCode, type, value, out string codigoError, out string descripcionError))
            return Result.Fail<OtherTaxId>(Error.Validation(codigoError, descripcionError));

        return Result.Ok(new OtherTaxId(value, type, countryCode));
    }
}