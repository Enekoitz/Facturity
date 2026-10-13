using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceLineError
{
    internal static readonly Error NullOrEmptyDescription =
        Error.Validation("InvoiceLine.NullOrEmptyDescription", 
            $"The invoice line description can`t be null or empty.");
    
    internal static readonly Error ExceededMaximumLengthDescription =
        Error.Validation("InvoiceLine.ExceededMaximumLengthDescription", 
            $"The invoice line description can`t be greater than 100 characters.");
    
    internal static readonly Error NegativeOrZeroUnits =
        Error.Validation("InvoiceLine.NegativeOrZeroUnits", 
            $"The invoice line units can`t be zero or negative.");
    
    internal static readonly Error ExceededMaximumDecimalPlacesUnits =
        Error.Validation("InvoiceLine.ExceededMaximumDecimalPlacesUnits", 
            $"The unit decimal places can`t be greater than {Parameters.UnitsMaxDecimalPlaces}.");
}