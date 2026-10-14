using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

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
        var invoiceLine_21_1 = CreateLine(units: 2m, unitPrice: 2.356m, taxRate: 21.00m).Value;
        var invoiceLine_21_2 = CreateLine(units: 14m, unitPrice: 3.34m, taxRate: 21m).Value;
        var invoiceLine_15_1 = CreateLine(units: 8.45m, unitPrice: 1.055m, taxRate: 15.00m).Value;
        var invoiceLine_15_2 = CreateLine(units: 34.2m, unitPrice: 5.23m, taxRate: 15m).Value;
        var invoiceLine_4_1 = CreateLine(units: 1m, unitPrice: 2.34m, taxRate: 4m).Value;
        
        var invoiceLines = 
            new List<InvoiceLine> {invoiceLine_21_1, invoiceLine_21_2, invoiceLine_15_1, invoiceLine_15_2, invoiceLine_4_1};

        var result = BreakdownLine.CreateList(invoiceLines);
        
        Assert.True(result.IsSuccess);
        
        // The number of lines grouped by tax rate must equal the number of breakdown lines.
        Assert.Equal(invoiceLines.GroupBy(l => l.TaxRate).Count(), result.Value.Count);
        
        // The breakdown lines (tax bases, tax amounts, and total amounts) must match the invoice line totals.
        var expectedGroups = invoiceLines.GroupBy(l => l.TaxRate);
        foreach (var group in expectedGroups)
        {
            var breakdownLine = result.Value.FirstOrDefault(l => l.TaxRate == group.Key);
    
            Assert.NotNull(breakdownLine);

            Assert.Equal(group.Sum(l => l.TaxBase), breakdownLine.TaxBase);
            Assert.Equal(group.Sum(l => l.TaxAmount), breakdownLine.TaxAmount);
            Assert.Equal(group.Sum(l => l.TotalAmount), breakdownLine.TotalAmount);
        }
        
        Assert.Equal(invoiceLines.Sum(l => l.TaxBase), result.Value.Sum(l => l.TaxBase));
        Assert.Equal(invoiceLines.Sum(l => l.TaxAmount), result.Value.Sum(l => l.TaxAmount));
        Assert.Equal(invoiceLines.Sum(l => l.TotalAmount), result.Value.Sum(l => l.TotalAmount));
        
        // The returned value must be of a read-only list type.
        Assert.IsType<IReadOnlyList<BreakdownLine>>(result.Value, exactMatch:false);
    }

    [Fact]
    public void CreateList_WithEmptyInvoiceLinesList_ShouldFailAndReturnTypedError()
    {
        IReadOnlyList<InvoiceLine> emptyInvoiceLines = [];

        var result = BreakdownLine.CreateList(emptyInvoiceLines);
        
        Assert.True(result.IsFailure);
        Assert.Contains(BreakdownLineError.EmptyInvoiceLinesList, result.Errors);
        Assert.Single(result.Errors);
    }
}