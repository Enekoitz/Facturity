namespace Facturity.Modules.SharedKernel.Abstractions;

public enum SoftDeleteResultTypes
{
    Deleted,
    NotFound,
    AlreadyDeleted
}

public sealed record SoftDeleteResult(SoftDeleteResultTypes Result, DateTimeOffset? DeletedAtUtc = null)

{
    public static SoftDeleteResult Deleted(DateTimeOffset deletedAtUtc) =>
        new(SoftDeleteResultTypes.Deleted, deletedAtUtc);

    public static SoftDeleteResult NotFound() =>
        new(SoftDeleteResultTypes.NotFound);

    public static SoftDeleteResult AlreadyDeleted(DateTimeOffset deletedAtUtc) =>
        new(SoftDeleteResultTypes.AlreadyDeleted, deletedAtUtc);
}