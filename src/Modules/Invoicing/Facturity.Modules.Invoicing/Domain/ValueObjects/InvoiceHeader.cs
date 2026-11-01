using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed record InvoiceHeader
{
    internal Guid SeriesId { get; private init; }
    internal bool IsSimplified { get; private init; }
    internal DateTimeOffset? IssuedAtUtc { get; private init; }
    internal ulong? Number { get; private init; } //Only loaded by ORM materialization
    public InvoiceSeries Series { get; private set; } = null!;
    
    private InvoiceHeader(InvoiceSeries series, bool isSimplified)
    {
        Series = series;
        IsSimplified = isSimplified;
    }

    private InvoiceHeader() { } //ORM Materialization

    internal static Result<InvoiceHeader> Create(InvoiceSeries series, bool isSimplified)
    {
        if (series is null)
            return Result.Fail<InvoiceHeader>(InvoiceHeaderError.SeriesRequired);
            
        return Result.Ok(new InvoiceHeader(series, isSimplified));
    }

    internal InvoiceHeader WithIssuance(DateTimeOffset issuedAt) =>
        this with
        {
            IssuedAtUtc = issuedAt.ToUniversalTime()
        };
}