using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

internal static class TaxIdErrors
{
    internal static readonly Error Invalid = Error.Validation(
        "Customer.TaxId.Invalid",
        "Tax ID invalid data.");
    
    internal static readonly Error SpanishTaxIdSubtypeNotFound =
        Error.NotFound("Customer.TaxId.SpanishTaxIdSubtype.NotFound", 
            "Spanish tax ID subtype (DNI, NIE, CIF) not found.");
}