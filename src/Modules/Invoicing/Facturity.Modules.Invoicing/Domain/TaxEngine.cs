namespace Facturity.Modules.Invoicing.Domain;

internal static class TaxEngine
{
    internal static decimal CalculateTaxBase(decimal units, UnitPrice unitPrice) =>
        InvoiceRounding.Round(units * unitPrice.Price);

    internal static decimal CalculateTaxAmount(Money taxBase, TaxRate taxRate) =>
        InvoiceRounding.Round(taxBase.Amount * taxRate.Percentage / 100m);
}