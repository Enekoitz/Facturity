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
    
    internal static List<InvoiceLine> GetInvoiceLinesSampleList() =>
    [
        CreateLine(units: 2m, unitPrice: 2.356m, taxRate: 21.00m).Value,
        CreateLine(units: 14m, unitPrice: 3.34m, taxRate: 21m).Value,
        CreateLine(units: 8.45m, unitPrice: 1.055m, taxRate: 15.00m).Value,
        CreateLine(units: 34.2m, unitPrice: 5.23m, taxRate: 15m).Value,
        CreateLine(units: 1m, unitPrice: 2.34m, taxRate: 4m).Value
    ];
    
    internal static Result<InvoiceSeries> CreateSeries(
        string name = "DFB_MLC") =>
        InvoiceSeries.Create(name);
    
    internal static Result<InvoiceHeader> CreateInvoiceHeader(
        Guid seriesId,
        bool isSimplified,
        DateTimeOffset? issuedAtUtc,
        TimeProvider fakeTimeProvider) =>
        InvoiceHeader.Create(seriesId, isSimplified, issuedAtUtc, fakeTimeProvider);
}