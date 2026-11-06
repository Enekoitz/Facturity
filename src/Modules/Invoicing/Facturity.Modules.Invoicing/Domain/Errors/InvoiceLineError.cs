using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceLineError
{
    internal static readonly Error InvoiceLineIdInvalid =
        Error.Validation("InvoiceLine.InvoiceLineIdInvalid", 
            $"Invoice line Id is invalid.");
    
    internal static readonly Error InvoiceIdInvalid =
        Error.Validation("InvoiceLine.InvoiceIdInvalid", 
            $"Invoice Id is invalid.");
    
    internal static readonly Error DescriptionEmpty =
        Error.Validation("InvoiceLine.DescriptionEmpty", 
            $"Invoice line description is empty.");
    
    internal static readonly Error DescriptionTooLong =
        Error.Validation("InvoiceLine.DescriptionTooLong", 
            $"Invoice line description exceeds the maximum length of {InvoiceLine.DescriptionMaxLength} characters.");
    
    internal static readonly Error UnitsNegativeOrZero =
        Error.Validation("InvoiceLine.UnitsNegativeOrZero", 
            $"The invoice line units can`t be zero or negative.");
    
    internal static readonly Error UnitsTooMuchDecimalPlaces =
        Error.Validation("InvoiceLine.UnitsTooMuchDecimalPlaces", 
            $"The unit decimal places can`t be greater than {InvoiceLine.UnitsMaxDecimalPlaces}.");
}