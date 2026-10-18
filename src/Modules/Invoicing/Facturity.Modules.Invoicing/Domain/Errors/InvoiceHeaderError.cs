using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceHeaderError
{
    internal static readonly Error SeriesIdEmpty = 
        Error.Validation("InvoiceHeader.SeriesIdEmpty",
            "The series id, Guid type, cannot be empty.");
}