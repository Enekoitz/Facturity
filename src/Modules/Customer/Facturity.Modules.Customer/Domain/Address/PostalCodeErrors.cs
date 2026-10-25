using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class PostalCodeErrors
{
    internal static readonly Error Empty =
        Error.Validation("Customer.Address.PostalCode.Empty",
            "Postal code is required.");
    
    internal static readonly Error TooLong =
        Error.Validation("Customer.Address.PostalCode.TooLong",
            $"Postal code must not exceed {Parameters.Address.PostalCodeExactLength} characters.");
    
    internal static readonly Error NotNumber =
        Error.Validation("Customer.Address.PostalCode.NotDigit",
            "Postal code must be a number.");
}