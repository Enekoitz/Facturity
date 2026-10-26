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
            var result = SpanishTaxId.Create(taxIdValue);
            return result.IsSuccess
                ? Result.Ok<ITaxId>(result.Value)
                : Result.Fail<ITaxId>(result.Errors);
        }
        
        if (taxIdCountryCode is not null)
        {
            var result = OtherTaxId.Create(taxIdValue, taxIdType, taxIdCountryCode);
            return result.IsSuccess
                ? Result.Ok<ITaxId>(result.Value)
                : Result.Fail<ITaxId>(result.Errors);
        }

        return Result.Fail<ITaxId>(TaxIdErrors.Invalid);
    }
}
