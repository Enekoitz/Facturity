using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class MoneyError
{
    internal static readonly Error InvalidAmountNegativeOrZero =
        Error.Validation("Money.InvalidAmountNegativeOrZero", 
            "The amount can't be negative or zero.");
}