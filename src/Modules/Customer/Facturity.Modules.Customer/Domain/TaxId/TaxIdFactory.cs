using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class TaxIdFactory : ITaxIdFactory
{
    public Result<ITaxId> Create(
        string taxIdValue,
        TaxIdType taxIdType,
        string? taxIdCountryCode)
    {
        if (taxIdType == TaxIdType.SpanishTaxId)
        {
            var nifEspanol = SpanishTaxId.Crear(taxIdValue);
            return nifEspanol.IsSuccess
                ? Result.Ok<ITaxId>(nifEspanol.Value)
                : Result.Fail<ITaxId>(nifEspanol.Errors);
        }
        
        if (taxIdCountryCode is not null)
        {
            var idOtroExtranjero = OtherTaxId.Create(taxIdValue, taxIdType, taxIdCountryCode);
            return idOtroExtranjero.IsSuccess
                ? Result.Ok<ITaxId>(idOtroExtranjero.Value)
                : Result.Fail<ITaxId>(idOtroExtranjero.Errors);
        }

        return Result.Fail<ITaxId>(TaxIdErrors.Invalid);
    }
}
