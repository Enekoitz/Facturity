using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed record InvoiceHeader
{
    internal InvoiceSeries Series { get; private init; }
    internal bool IsSimplified { get; private init; }
    internal DateTimeOffset? IssuedAtUtc { get; private init; }
    internal uint? Number { get; private init; } //Only loaded by ORM materialization

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