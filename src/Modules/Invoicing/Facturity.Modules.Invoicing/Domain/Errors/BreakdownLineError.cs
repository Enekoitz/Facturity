using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal static class BreakdownLineError
{
    internal static readonly Error EmptyInvoiceLinesList =
        Error.Validation("BreakdownLine.EmptyInvoiceLinesList", 
            $"The invoice lines list must not be empty.");
}