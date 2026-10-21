using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceDataError
{
    internal static readonly Error NullOrEmptyDescription =
        Error.Validation("InvoiceData.NullOrEmptyDescription", 
            $"The invoice data description can`t be null or empty.");
    
    internal static readonly Error ExceededMaximumLengthDescription =
        Error.Validation("InvoiceData.ExceededMaximumLengthDescription", 
            $"The invoice data description can`t be greater than {InvoiceData.DescriptionMaxLength} characters.");
    
    internal static readonly Error EmptyInvoiceLinesList =
        Error.Validation("InvoiceData.EmptyInvoiceLinesList", 
            $"The list of invoice lines must not be empty.");
    
    internal static readonly Error FutureDateTransactionAtUtc = 
        Error.Validation("InvoiceData.FutureDateTransactionAtUtc",
        "The transaction date and time must not be in the future.");
}