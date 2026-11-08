using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class IssuanceInfoError
{
    internal static readonly Error IssueDateInvalid = 
        Error.Validation("Invoice.IssueDateInvalid",
            "The date of issuance is invalid.");
    
    internal static readonly Error IssueDateIsFuture = 
        Error.Validation("Invoice.IssueDateIsFuture",
            "The date of issuance must not be a future date.");
}