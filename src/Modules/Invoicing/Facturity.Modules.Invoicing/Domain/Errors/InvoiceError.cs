using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceError
{
    internal static readonly Error IssueDateAtInvoiceDraft =
        Error.Validation("Invoice.IssueDateAtInvoiceDraft", 
            $"An invoice draft cannot have issued date.");
    
    internal static readonly Error SeriesNumberSetAtDomain  =
        Error.Validation("Invoice.SeriesNumberSetAtDomain", 
            $"An invoice cannot have a number set in the domain.");
    
    internal static readonly Error CustomerIdAtSimplifiedInvoice =
        Error.Validation("Invoice.CustomerIdAtSimplifiedInvoice", 
            $"A simplified invoice cannot have customer id.");
    
    internal static readonly Error MissingCustomerIdAtNoSimplifiedInvoice =
        Error.Validation("Invoice.MissingCustomerIdAtNoSimplifiedInvoice", 
            $"A non-simplified invoice must have customer id.");
    
    internal static readonly Error FutureIssueDate = 
        Error.Validation("InvoiceHeader.FutureIssueDate",
            "The date of issuance must not be a future date.");
    
    internal static readonly Error NotInDraft =
        Error.Validation("Invoice.NotInDraft", 
            $"The invoice is not in draft.");
    
    internal static readonly Error MissingIssueDate =
        Error.Validation("Invoice.MissingIssueDate", 
            $"The invoice is not in draft.");
    
    internal static readonly Error NullInvoiceHeader =
        Error.Validation("Invoice.NullInvoiceHeader", 
            $"The invoice header is null.");
    
    internal static readonly Error NullInvoiceData =
        Error.Validation("Invoice.NullInvoiceData", 
            $"The invoice data is null.");
    
    internal static readonly Error NoInvoiceLines =
        Error.Validation("Invoice.NoInvoiceLine", 
            $"There are no invoice lines.");
    
}