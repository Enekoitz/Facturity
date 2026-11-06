using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Invoicing.Domain;

internal sealed record IssuanceInfo
{
    internal DateTimeOffset IssuedAtUtc { get; private init; }
    internal ulong? Number { get; private init; } = null!;
    
    private IssuanceInfo(DateTimeOffset issuedAtUtc)
    {
        IssuedAtUtc = issuedAtUtc;
    }

    private IssuanceInfo() { }

    internal static Result<IssuanceInfo> Create(DateTimeOffset issuedAtUtc, DateTimeOffset nowUtc)
    {
        if (issuedAtUtc == default)
            return Result.Fail<IssuanceInfo>(IssuanceInfoError.IssueDateInvalid);
        
        if (issuedAtUtc > nowUtc)
            return Result.Fail<IssuanceInfo>(IssuanceInfoError.IssueDateIsFuture);
            
        return Result.Ok(new IssuanceInfo(issuedAtUtc));
    }
}