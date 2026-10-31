using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceSeriesErrors
{
    internal static readonly Error NameEmpty =
        Error.Validation("InvoiceSeries.NameEmpty", 
            $"Name is required.");
    
    internal static readonly Error NameTooLong =
        Error.Validation("InvoiceSeries.NameTooLong", 
            $"Name must not exceed {InvoiceSeries.NameMaxLength} characters.");

    internal static readonly Error DescriptionTooLong =
        Error.Validation("InvoiceSeries.DescriptionTooLong", 
            $"Description must not exceed {InvoiceSeries.DescriptionMaxLength} characters.");
}