using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal record BreakdownLine
{
    internal Money TaxBase { get; private init; }
    internal TaxRate TaxRate { get; private init; }
    internal Money TaxAmount { get; private init; }
    internal Money TotalAmount{ get; private init; }

    private BreakdownLine(Money taxBase, TaxRate taxRate, Money taxAmount, Money totalAmount)
    {
        TaxBase = taxBase;
        TaxRate = taxRate;
        TaxAmount = taxAmount;
        TotalAmount = totalAmount;
    }
    
    private BreakdownLine(){ }

    internal static IReadOnlyList<BreakdownLine> CreateList(IReadOnlyList<InvoiceLine> invoiceLines)
    {
        var breakdownLines = 
            invoiceLines
                .GroupBy(l => l.TaxRate)
                .OrderByDescending(g => g.Key.Percentage)
                .Select(g => new BreakdownLine(
                        taxBase: g.Sum(l => l.TaxBase),
                        taxRate: g.Key,
                        taxAmount: g.Sum(l => l.TaxAmount),
                        totalAmount: g.Sum(l => l.TotalAmount)
                    ))
                .ToList();

        return breakdownLines.AsReadOnly();
    }
}