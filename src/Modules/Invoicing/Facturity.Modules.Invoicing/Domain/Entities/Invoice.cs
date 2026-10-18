using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal class Invoice : AggregateRoot<Guid>
{
    internal Guid? CustomerId { get; private set; }
    internal InvoiceHeader? Header { get; private set; }
    internal InvoiceData? Data { get; private set; }
    internal IReadOnlyList<BreakdownLine> TaxBreakdown => GetTaxBreakdown();
    internal InvoiceStatus Status { get; private set; }

    private Invoice(Guid id, TenantId tenantId, Guid? customerId, InvoiceHeader? header, 
                    InvoiceData? data, InvoiceStatus status) : base(id, tenantId)
    {
        CustomerId = customerId;
        Header = header;
        Data = data;
        Status = status;
    }
    
    private Invoice() { } //ORM Materialization

    internal static Result<Invoice> CreateDraft(TenantId tenantId, Guid? customerId, 
                                                InvoiceHeader? header, InvoiceData? data)
    {
        var errors = ValidateDraft(customerId, header);
        if (errors.Count > 0)
            return Result.Fail<Invoice>(errors);

        return Result.Ok(new Invoice(Guid.NewGuid(), tenantId, customerId, header, data, InvoiceStatus.Draft));
    }

    internal Result UpdateDraft(Guid? customerId, InvoiceHeader? header, InvoiceData? data)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);

        var errors = ValidateDraft(customerId, header);
        if (errors.Count > 0)
            return Result.Fail(errors);

        CustomerId = customerId;
        Header = header;
        Data = data;
        return Result.Ok();
    }

    internal Result Issue(DateTimeOffset issuedAtUtc, DateTimeOffset now)
    {
        if (Status != InvoiceStatus.Draft)
            return Result.Fail(InvoiceError.NotInDraft);

        var errors = ValidateForIssue();

        if (issuedAtUtc > now)
            errors.Add(InvoiceError.FutureIssueDate);
            
        if (errors.Count > 0)
            return Result.Fail(errors);
        
        //The invoice number (InvoiceNumber) is set within a database transaction to ensure consistency and sequential
        //ordering, thereby avoiding race conditions and concurrency conflicts.
        
        Header = Header!.WithIssuance(issuedAtUtc); 
        Status = InvoiceStatus.Issued;
        return Result.Ok();
    }

    private static List<Error> ValidateDraft(Guid? customerId, InvoiceHeader? header)
    {
        var errors = new List<Error>();
        if (header is null) return errors;

        if (header.IsSimplified && customerId is not null)
            errors.Add(InvoiceError.CustomerIdAtSimplifiedInvoice);
        
        if (header.IssuedAtUtc is not null)
            errors.Add(InvoiceError.IssueDateAtInvoiceDraft);
        
        if (header.Number is not null)
            errors.Add(InvoiceError.SeriesNumberSetAtDomain);

        return errors;
    }

    private List<Error> ValidateForIssue()
    {
        var errors = new List<Error>();

        if (Header is null)
            errors.Add(InvoiceError.NullInvoiceHeader);
        else if (!Header.IsSimplified && CustomerId is null)
            errors.Add(InvoiceError.MissingCustomerIdAtNoSimplifiedInvoice);

        if (Data is null)
            errors.Add(InvoiceError.NullInvoiceData);
        else if (Data.Lines.Count == 0)
            errors.Add(InvoiceError.NoInvoiceLines);

        return errors;
    }

    internal IReadOnlyList<BreakdownLine> GetTaxBreakdown() =>
        Data is null
            ? []
            : BreakdownLine.CreateList(Data.Lines);
}