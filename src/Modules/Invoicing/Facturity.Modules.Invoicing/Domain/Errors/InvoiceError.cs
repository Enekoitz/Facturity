using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceError
{
    internal static readonly Error IdInvalid =
        Error.Validation("Invoice.IdInvalid", 
            "Invoice identifier is invalid.");
    
    internal static readonly Error InvalidCurrencyCode =
        Error.Validation("Invoice.InvalidCurrencyCode", 
            "The currency code is invalid. It must be 3 characters long.");
    
    internal static readonly Error SeriesInvalid =
        Error.Validation("Invoice.SeriesInvalid", 
            "Series Id is invalid.");
    
    internal static readonly Error SeriesRequired = 
        Error.Validation("Invoice.SeriesRequired",
            "Invoice series is required.");
    
    internal static readonly Error LineEmpty =
        Error.Validation("Invoice.LineEmpty", 
            "Invoice line is empty.");
    
    internal static readonly Error LineWithDifferentInvoiceIdentifier =
        Error.Validation("Invoice.LineWithDifferentInvoiceIdentifier", 
            "The invoice identifier for the invoice line differs from that of the invoice.");
    
    internal static readonly Error LineWithDifferentTenantIdentifier =
        Error.Validation("Invoice.LineWithDifferentTenantIdentifier", 
            "The tenant identifier for the invoice line differs from that of the invoice.");
    
    internal static readonly Error LinesCollectionEmpty =
        Error.Validation("Invoice.LinesCollectionEmpty", 
            "The invoice lines collection is empty.");
    
    internal static readonly Error IssueDateAtInvoiceDraft =
        Error.Validation("Invoice.IssueDateAtInvoiceDraft", 
            $"An invoice draft cannot have issued date.");
    
    internal static readonly Error SeriesNumberSetAtDomain  =
        Error.Validation("Invoice.SeriesNumberSetAtDomain", 
            $"An invoice cannot have a number set in the domain.");
    
    internal static readonly Error CustomerIdInvalid =
        Error.Validation("Invoice.CustomerIdInvalid", 
            $"CustomerId is invalid.");
    
    internal static readonly Error CustomerIdNoAtSimplifiedInvoice =
        Error.Validation("Invoice.CustomerIdNoAtSimplifiedInvoice", 
            $"A simplified invoice must not have a customer.");
    
    internal static readonly Error CustomerIdNeededAtNoSimplifiedInvoice =
        Error.Validation("Invoice.CustomerIdNeededAtNoSimplifiedInvoice", 
            $"A non-simplified invoice must have a customer.");
    
    internal static readonly Error TransactionDateInvalid =
        Error.Validation("Invoice.TransactionDateInvalid", 
            $"Transaction date is invalid.");
    
    internal static readonly Error TransactionDateIsFuture =
        Error.Validation("Invoice.TransactionDateIsFuture", 
            $"Transaction date must not be a future date.");
    
    internal static readonly Error IssueDateInvalid =
        Error.Validation("Invoice.IssueDateInvalid", 
            $"Issue date is invalid.");
    
    internal static readonly Error IssueDateFuture = 
        Error.Validation("Invoice.IssueDateFuture",
            "Issue date must not be a future date.");
    
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
    
    internal static readonly Error InvoiceLinesEmpty =
        Error.Validation("Invoice.InvoiceLinesEmpty", 
            $"Invoice lines is empty. At least one line is required.");
    
    internal static readonly Error DescriptionTooLong =
        Error.Validation("Invoice.ExceededMaximumLengthDescription", 
            $"Description can`t be greater than {Invoice.DescriptionMaxLength} characters.");
    
    internal static readonly Error EmptyInvoiceLinesList =
        Error.Validation("Invoice.EmptyInvoiceLinesList", 
            $"The list of invoice lines must not be empty.");
    
    internal static readonly Error FutureDateTransactionAtUtc = 
        Error.Validation("Invoice.FutureDateTransactionAtUtc",
            "Transaction date and time must not be in the future.");
}