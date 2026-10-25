using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record SpanishTaxId : ITaxId
{
    public string Value { get; }
    public SpanishTaxIdSubtype Subtype { get; }
    public TaxIdType TaxIdType { get; }
    public bool? IsLegalEntity => Subtype == SpanishTaxIdSubtype.Cif;

    protected SpanishTaxId() { }

    private SpanishTaxId(string value, SpanishTaxIdSubtype subtype)
    {
        Value = value;
        Subtype = subtype;
        TaxIdType = TaxIdType.SpanishTaxId;
    }

    public static Result<SpanishTaxId> Create(string value)
    {
        var isValid = TaxIdValidations.IsValidSpanishTaxId(value, out var errorCode, out var errorDescription);
        if (!isValid)
            return Result.Fail<SpanishTaxId>(Error.Validation(errorCode, errorDescription));

        var subtypeResult = SpanishTaxIdService.GetSubtype(value);
        if (subtypeResult.IsFailure)
            return Result.Fail<SpanishTaxId>(subtypeResult.Errors);

        return Result.Ok(new SpanishTaxId(value, subtypeResult.Value));
    }

}