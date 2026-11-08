using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal class Invoice : AggregateRoot<Guid>
{
    internal const string DefaultCurrency = "EUR";
    internal const int CurrencyExactLength = 3;
    internal const int DescriptionMaxLength = 250;
    
    private readonly List<InvoiceLine> _lines = [];
    
    internal Guid? CustomerId { get; private set; }
    internal Guid? SeriesId { get; private set; }
    internal InvoiceStatus Status { get; private set; }
    internal bool IsSimplified { get; private init; }
    internal IssuanceInfo? IssuanceInfo { get; private set; } = null!;
    internal DateTimeOffset? TransactionAtUtc{ get; private set; }
    internal string? Description { get; private set; }
    internal IReadOnlyList<InvoiceLine> Lines => _lines.AsReadOnly();
    internal Money TotalAmount() => Lines.Sum(i => i.TotalAmount);
    internal IReadOnlyList<BreakdownLine> TaxBreakdown => GetTaxBreakdown();
    internal string Currency { get; private set; } = DefaultCurrency;

    private Invoice(Guid id, TenantId tenantId, InvoiceStatus status, bool isSimplified) 
        : base(id, tenantId)
    {
        IsSimplified = isSimplified;
        Status = status;
    }
    
    private Invoice() { }

    internal static Result<Invoice> CreateDraft(Guid id, TenantId tenantId, bool isSimplified)
    {
        List<Error> errors = [];
        
        if (id  == Guid.Empty)
            errors.Add(InvoiceError.IdInvalid);
  
        return errors.Count > 0 
            ? Result.Fail<Invoice>(errors) 
            : Result.Ok(new Invoice(id, tenantId, InvoiceStatus.Draft, isSimplified));
    }

    internal Result ChangeCustomer(Guid? customerId)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);

        if (customerId == Guid.Empty)
            return Result.Fail(InvoiceError.CustomerIdInvalid);
        
        if (IsSimplified && customerId is not null)
            return Result.Fail(InvoiceError.CustomerIdNoAtSimplifiedInvoice);
            
        CustomerId = customerId;
        
        return Result.Ok();
    }
    
    internal Result ChangeSeries(Guid? seriesId)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);

        if (seriesId is not null && seriesId == Guid.Empty)
            return Result.Fail(InvoiceError.SeriesInvalid);
            
        SeriesId = seriesId;
        
        return Result.Ok();
    }
    
    internal Result ChangeDetails(string? description, DateTimeOffset? transactionAtUtc, DateTimeOffset? nowUtc)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);
        
        List<Error> errors = [];
        
        description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        if (description?.Length > DescriptionMaxLength)
            errors.Add(InvoiceError.DescriptionTooLong);
        
        if (transactionAtUtc is not null && transactionAtUtc == DateTimeOffset.MinValue)
            errors.Add(InvoiceError.TransactionDateInvalid);
        else if (transactionAtUtc > nowUtc)
            errors.Add(InvoiceError.TransactionDateIsFuture);
        
        if (errors.Count > 0)
            return Result.Fail(errors);
            
        Description = description;
        TransactionAtUtc = transactionAtUtc;
        
        return Result.Ok();
    }
    
    internal Result ChangeCurrency(string? currencyCode)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);
        
        currencyCode ??= DefaultCurrency;
        currencyCode = currencyCode.Trim().ToUpper();
        
        if (currencyCode.Length != CurrencyExactLength)
            return Result.Fail(InvoiceError.InvalidCurrencyCode);
        
        Currency = currencyCode;
        
        return Result.Ok();
    }
    
    internal Result AddLine(InvoiceLine line)
    {
        List<Error> errors = [];
        
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);
        
        if (line is null)
            errors.Add(InvoiceError.LineEmpty);
        
        if (Id != line?.InvoiceId)
            errors.Add(InvoiceError.LineWithDifferentInvoiceIdentifier);
        
        if (TenantId != line?.TenantId)
            errors.Add(InvoiceError.LineWithDifferentTenantIdentifier);
        
        if(errors.Count > 0)
            return Result.Fail(errors);
            
        _lines.Add(line);
        
        return Result.Ok();
    }
    
    internal Result AddLines(IEnumerable<InvoiceLine> lines)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);
    
        var linesList = lines?.ToList();

        if (linesList is null || linesList.Count == 0 || linesList.Any(l => l is null))
            return Result.Fail(InvoiceError.LinesCollectionEmpty);
        
        _lines.AddRange(linesList);
    
        return Result.Ok();
    }

    internal Result Issue(DateTimeOffset issuedAtUtc, DateTimeOffset nowUtc)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);

        List<Error> errors = [];

        if (IsSimplified && CustomerId is not null)
            errors.Add(InvoiceError.CustomerIdNoAtSimplifiedInvoice);
        else if (!IsSimplified && CustomerId is null)
            errors.Add(InvoiceError.CustomerIdNeededAtNoSimplifiedInvoice);

        if (SeriesId is null)
            errors.Add(InvoiceError.SeriesRequiredAtIssuanceTime);

        if (Description is null)
            errors.Add(InvoiceError.DescriptionRequiredAtIssuanceTime);
        
        if (_lines.Count == 0)
            errors.Add(InvoiceError.InvoiceLinesEmpty);

        var issuanceInfo = IssuanceInfo.Create(issuedAtUtc, nowUtc);
        if (issuanceInfo.IsFailure)
            errors.AddRange(issuanceInfo.Errors);
                
        if (errors.Count > 0)
            return Result.Fail(errors);
        
        IssuanceInfo = issuanceInfo.Value;
        Status = InvoiceStatus.Issued;

        return Result.Ok();
    }

    internal IReadOnlyList<BreakdownLine> GetTaxBreakdown() =>
        BreakdownLine.CreateList(_lines);
}