using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class AddressErrors
{
    internal static readonly Error StreetEmpty =
        Error.Validation("Customer.Address.StreetEmpty", 
            "Street is required.");
}