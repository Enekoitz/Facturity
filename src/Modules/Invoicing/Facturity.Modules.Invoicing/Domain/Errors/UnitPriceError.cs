using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class UnitPriceError
{
    internal static readonly Error InvalidCurrencyCode =
        Error.Validation("UnitPrice.InvalidCurrencyCode", 
            $"The unit price code is not valid. It must be {Parameters.CurrencyExactLength} characters long.");
    
    internal static readonly Error InvalidAmountNegativeOrZero =
        Error.Validation("UnitPrice.InvalidAmountNegativeOrZero", 
            $"The unit price cannot be negative or zero.");
    
    internal static readonly Error ExceededMaximumDecimalPlaces =
        Error.Validation("UnitPrice.ExceededMaximumDecimalPlaces", 
            $"The unit price cannot have more than {Parameters.UnitPriceMaxDecimalPlaces} decimal places.");
}