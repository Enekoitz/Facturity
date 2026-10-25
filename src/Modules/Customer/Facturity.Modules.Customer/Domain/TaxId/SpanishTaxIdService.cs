using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public static class SpanishTaxIdService
{
    public static Result<SpanishTaxIdSubtype> GetSubtype(string value)
    {
        if (TaxIdValidations.IsDni(value))
            return Result.Ok(SpanishTaxIdSubtype.Dni);

        if (TaxIdValidations.IsNie(value))
            return Result.Ok(SpanishTaxIdSubtype.Nie);

        if (TaxIdValidations.IsCif(value))
            return Result.Ok(SpanishTaxIdSubtype.Cif);

        return Result.Fail<SpanishTaxIdSubtype>(TaxIdErrors.SpanishTaxIdSubtypeNotFound);
    }
}
