using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class TaxRateError
{
    internal static readonly Error InvalidTaxRate =
        Error.Validation("Money.InvalidTaxRate", 
            "The tax rate is invalid. It must be greater than or equal to 0 and less than or equal to 100.");
}