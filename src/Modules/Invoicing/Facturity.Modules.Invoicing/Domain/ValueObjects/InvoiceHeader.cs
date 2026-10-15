using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed record InvoiceHeader
{
    internal Guid SeriesId { get;  private init; }
    internal uint? Number { get; } = null;
    internal bool IsSimplified { get; private init; }
    internal DateTimeOffset? IssuedAtUtc { get;  private init; }

    private InvoiceHeader(Guid seriesId, bool isSimplified, DateTimeOffset? issuedAtUtc)
    {
        SeriesId = seriesId;
        IsSimplified = isSimplified;
        IssuedAtUtc = issuedAtUtc;
    }
    
    private InvoiceHeader(){ }

    internal static Result<InvoiceHeader> Create(
        Guid seriesId,
        bool isSimplified,
        DateTimeOffset? issuedAtUtc,
        TimeProvider timeProvider)
    {
        var errors = new List<Error>();
        
        if(seriesId == Guid.Empty)
            errors.Add(InvoiceHeaderError.SeriesIdGuiEmpty);

        if(issuedAtUtc is not null && issuedAtUtc > timeProvider.GetUtcNow())
            errors.Add(InvoiceHeaderError.FutureDateIssuedAtUtc);
        
        if(errors.Count > 0)
            return Result.Fail<InvoiceHeader>(errors);
     
        return Result.Ok(new InvoiceHeader(seriesId, isSimplified, issuedAtUtc));
    }
}