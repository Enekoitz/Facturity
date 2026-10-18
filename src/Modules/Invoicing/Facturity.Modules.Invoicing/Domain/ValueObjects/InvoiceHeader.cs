using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed record InvoiceHeader
{
    internal Guid SeriesId { get; private init; }
    internal bool IsSimplified { get; private init; }
    internal DateTimeOffset? IssuedAtUtc { get; private init; }
    internal uint? Number { get; private init; } //Only loaded by ORM materialization

    private InvoiceHeader(Guid seriesId, bool isSimplified)
    {
        SeriesId = seriesId;
        IsSimplified = isSimplified;
    }

    private InvoiceHeader() { } //ORM Materialization

    internal static Result<InvoiceHeader> Create(Guid seriesId, bool isSimplified)
    {
        if (seriesId == Guid.Empty)
            return Result.Fail<InvoiceHeader>(InvoiceHeaderError.SeriesIdEmpty);

        return Result.Ok(new InvoiceHeader(seriesId, isSimplified));
    }

    internal InvoiceHeader WithIssuance(DateTimeOffset issuedAt) =>
        this with
        {
            IssuedAtUtc = issuedAt.ToUniversalTime()
        };
}