using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceHeaderError
{
    internal static readonly Error SeriesRequired = 
        Error.Validation("InvoiceHeader.SeriesRequired",
            "Invoice series is required.");
}