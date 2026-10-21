using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

internal static class Build
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
    
    internal static Result<InvoiceHeader> Header(
        Guid seriesId,
        bool isSimplified) =>
        InvoiceHeader.Create(seriesId, isSimplified);
    
    internal static readonly DateTimeOffset Now = new(2026, 10, 18, 12, 0, 0, TimeSpan.Zero);

    internal static InvoiceData Data(int lines = 1) =>
        InvoiceData.Create("Data description",
                    Enumerable.Range(1, lines)
                                    .Select(i => InvoiceLine.Create(
                                        $"Line {i}", 
                                        units: i, 
                                        unitPrice: GetUnitPrice(10m * i), 
                                        taxRate: GetTaxRate(i % 2 == 0 ? 10m : 21m)).Value
                                    ).ToList().AsReadOnly(),
                    null, 
                    new DateTimeOffset(2026, 10, 18, 12, 0, 0, TimeSpan.Zero)).Value;

    internal static Invoice Draft(bool simplified = false) =>
        DraftWith(simplified ? null : Guid.NewGuid(), Header(Guid.NewGuid(),simplified).Value, Data());

    internal static Invoice DraftWith(Guid? customerId, InvoiceHeader? header, InvoiceData? data)
    {
        var result = Invoice.CreateDraft(Tenant(), customerId, header, data);
        Assert.True(result.IsSuccess);
        return result.Value;
    }

    internal static Invoice Issued()
    {
        var invoice = Draft();
        var result = invoice.Issue(Now, Now);
        Assert.True(result.IsSuccess);
        return invoice;
    }

    internal static TenantId Tenant()
        => TenantId.Crear(Guid.NewGuid()).Value;
}