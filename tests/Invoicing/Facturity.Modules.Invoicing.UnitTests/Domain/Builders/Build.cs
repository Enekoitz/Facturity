using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

internal static class Build
{
    internal static UnitPrice GetUnitPrice(decimal value) => UnitPrice.Create(value).Value;
    internal static TaxRate GetTaxRate(decimal value) => TaxRate.Create(value).Value;
    internal static Money GetMoney(decimal value) => Money.Create(value).Value;
    
    internal static Result<InvoiceLine> CreateLine(
        TenantId tenantId,
        Guid invoiceId,
        string? description = "Consulting",
        decimal units = 1m,
        decimal unitPrice = 10m,
        decimal taxRate = 21m) =>
        InvoiceLine.Create(Guid.CreateVersion7(), tenantId, invoiceId, description!, units, GetUnitPrice(unitPrice), GetTaxRate(taxRate));
    
    internal static List<InvoiceLine> GetInvoiceLinesSampleList(TenantId tenantId, Guid invoiceId) =>
    [
        CreateLine(tenantId, invoiceId, units: 2m, unitPrice: 2.356m, taxRate: 21.00m).Value,
        CreateLine(tenantId, invoiceId, units: 14m, unitPrice: 3.34m, taxRate: 21m).Value,
        CreateLine(tenantId, invoiceId, units: 8.45m, unitPrice: 1.055m, taxRate: 15.00m).Value,
        CreateLine(tenantId, invoiceId, units: 34.2m, unitPrice: 5.23m, taxRate: 15m).Value,
        CreateLine(tenantId, invoiceId, units: 1m, unitPrice: 2.34m, taxRate: 4m).Value
    ];
    
    internal static InvoiceLine GetValidInvoiceLine(TenantId tenantId, Guid invoiceId) =>
        CreateLine(tenantId, invoiceId, units: 145m, unitPrice: 0.256m, taxRate: 10.00m).Value;
    
    internal static Result<Invoice> CreateDraft(
        Guid id,
        TenantId tenantId,
        bool isSimplified)
    { 
        var result = Invoice.CreateDraft(id, tenantId, isSimplified);
        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(tenantId, result.Value.TenantId);
        Assert.Equal(isSimplified, result.Value.IsSimplified);
        return result;
    }
    
    internal static Result<InvoiceSeries> CreateSeries(
        TenantId tenantId,
        string name = "DFB_MLC",
        string? description = null)
    { 
        var id = Guid.CreateVersion7();
        var result = InvoiceSeries.Create(id, tenantId, name, description);

        return result;
    }
    
    internal static readonly DateTimeOffset NowUtc = new(2026, 10, 10, 12, 0, 0, TimeSpan.Zero);
    internal static readonly DateTimeOffset BeforeNowUtc = NowUtc.AddSeconds(-1);
    internal static readonly DateTimeOffset AfterNowUtc = NowUtc.AddSeconds(1);

    internal static TenantId Tenant()
        => TenantId.Create(Guid.CreateVersion7()).Value;
}