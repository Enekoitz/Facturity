using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceSeriesErrors
{
    internal static readonly Error NullOrEmptyName =
        Error.Validation("InvoiceSeries.NullOrEmptyName", 
            $"The invoice series name can`t be null or empty.");
    
    internal static readonly Error ExceededMaximumLengthName =
        Error.Validation("InvoiceSeries.ExceededMaximumLengthName", 
            $"The invoice series name can`t be greater than 100 characters.");
}