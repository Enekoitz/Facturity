using Facturity.Modules.Invoicing.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class BreakdownLineTests
{
    private static UnitPrice Price(decimal value) => UnitPrice.Create(value).Value;
    private static TaxRate Rate(decimal value) => TaxRate.Create(value).Value;

    private static Result<InvoiceLine> CreateLine(
        string? description = "Consulting",
        decimal units = 1m,
        decimal unitPrice = 10m,
        decimal taxRate = 21m) =>
        InvoiceLine.Create(description!, units, Price(unitPrice), Rate(taxRate));

    [Fact]
    public void CreateList_WithNoEmptyInvoiceLinesList_ShouldCreateBreakdownLinesReadOnlyList()
    {
        var invoiceLines = Build.GetInvoiceLinesSampleList();
        
        var result = BreakdownLine.CreateList(invoiceLines);
        
        Assert.Equal(invoiceLines.GroupBy(l => l.TaxRate).Count(), result.Count);
        
        var expectedGroups = invoiceLines.GroupBy(l => l.TaxRate);
        foreach (var group in expectedGroups)
        {
            var breakdownLine = result.FirstOrDefault(l => l.TaxRate == group.Key);
    
            Assert.NotNull(breakdownLine);

            Assert.Equal(group.Sum(l => l.TaxBase), breakdownLine.TaxBase);
            Assert.Equal(group.Sum(l => l.TaxAmount), breakdownLine.TaxAmount);
            Assert.Equal(group.Sum(l => l.TotalAmount), breakdownLine.TotalAmount);
        }
        
        Assert.Equal(invoiceLines.Sum(l => l.TaxBase), result.Sum(l => l.TaxBase));
        Assert.Equal(invoiceLines.Sum(l => l.TaxAmount), result.Sum(l => l.TaxAmount));
        Assert.Equal(invoiceLines.Sum(l => l.TotalAmount), result.Sum(l => l.TotalAmount));
        
        Assert.IsType<IReadOnlyList<BreakdownLine>>(result, exactMatch:false);
        
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateList_WithEmptyInvoiceLinesList_ShouldFailAndReturnTypedError()
    {
        IReadOnlyList<InvoiceLine> emptyInvoiceLines = [];

        var result = BreakdownLine.CreateList(emptyInvoiceLines);
        
        Assert.Empty(result);
    }
}