using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class UnitPriceError
{
    internal static readonly Error InvalidAmountNegativeOrZero =
        Error.Validation("UnitPrice.InvalidAmountNegativeOrZero", 
            $"The unit price cannot be negative or zero.");
    
    internal static readonly Error ExceededMaximumDecimalPlaces =
        Error.Validation("UnitPrice.ExceededMaximumDecimalPlaces", 
            $"The unit price cannot have more than {UnitPrice.MaxDecimalPlaces} decimal places.");
}