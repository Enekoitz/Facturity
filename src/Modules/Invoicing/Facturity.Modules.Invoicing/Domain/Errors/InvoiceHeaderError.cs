using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceHeaderError
{
    internal static readonly Error FutureDateIssuedAtUtc = 
        Error.Validation("InvoiceHeader.FutureDateIssuedAtUtc",
        "The issued date and time must not be in the future.");
    
    internal static readonly Error SeriesIdGuiEmpty = 
        Error.Validation("InvoiceHeader.SeriesIdGuiEmpty",
            "The series id, Guid type, cannot be empty.");
}