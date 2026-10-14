using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

internal static class Helper
{
    internal static UnitPrice GetUnitPrice(decimal value) => UnitPrice.Create(value).Value;
    internal static TaxRate GetTaxRate(decimal value) => TaxRate.Create(value).Value;
    internal static Money GetMoney(decimal value) => Money.Create(value).Value;
    

    internal static Result<InvoiceLine> CreateLine(
        string? description = "Consulting",
        decimal units = 1m,
        decimal unitPrice = 10m,
        decimal taxRate = 21m) =>
        InvoiceLine.Create(description!, units, GetUnitPrice(unitPrice), GetTaxRate(taxRate));
}