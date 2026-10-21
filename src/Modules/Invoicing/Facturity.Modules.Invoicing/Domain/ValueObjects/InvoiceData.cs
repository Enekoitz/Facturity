using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed record InvoiceData
{
    internal const int DescriptionMaxLength = 250;
    private readonly List<InvoiceLine> _lines;
    
    internal string Description { get; private init; }
    internal IReadOnlyList<InvoiceLine> Lines => _lines.AsReadOnly();

    internal Money TotalAmount() => Lines.Sum(i => i.TotalAmount);
    internal DateTimeOffset? TransactionAtUtc{ get; private init; }

    private InvoiceData(string description, List<InvoiceLine> invoiceLines, DateTimeOffset? transactionAtUtc)
    {
        Description = description;
        _lines = invoiceLines;
        TransactionAtUtc = transactionAtUtc;
    }
    
    private InvoiceData(){ }

    internal static Result<InvoiceData> Create(
        string description, 
        IReadOnlyList<InvoiceLine> invoiceLines, 
        DateTimeOffset? transactionAtUtc,
        DateTimeOffset now)
    {
        var errors = new List<Error>();

        var trimmedDescription = description?.Trim();

        if (string.IsNullOrEmpty(trimmedDescription))
            errors.Add(InvoiceDataError.NullOrEmptyDescription);
        else if (trimmedDescription.Length > DescriptionMaxLength)
            errors.Add(InvoiceDataError.ExceededMaximumLengthDescription);
        
        if(invoiceLines is null || !invoiceLines.Any())
            errors.Add(InvoiceDataError.EmptyInvoiceLinesList);
        
        if(transactionAtUtc > now)
            errors.Add(InvoiceDataError.FutureDateTransactionAtUtc);
        
        if(errors.Count > 0)
            return Result.Fail<InvoiceData>(errors);
     
        return Result.Ok(new InvoiceData(trimmedDescription!, invoiceLines.ToList(), transactionAtUtc));
    }
}