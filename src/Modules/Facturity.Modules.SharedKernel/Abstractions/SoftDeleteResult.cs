namespace Facturity.Modules.SharedKernel.Abstractions;

public enum SoftDeleteResultTypes
{
    Deleted,
    NotFound,
    AlreadyDeleted
}

public sealed record SoftDeleteResult(SoftDeleteResultTypes Result, DateTime? DeletedAtUtc = null)

{
    public static SoftDeleteResult Deleted(DateTime deletedAtUtc) =>
        new(SoftDeleteResultTypes.Deleted, deletedAtUtc);

    public static SoftDeleteResult NotFound() =>
        new(SoftDeleteResultTypes.NotFound);

    public static SoftDeleteResult AlreadyDeleted(DateTime deletedAtUtc) =>
        new(SoftDeleteResultTypes.AlreadyDeleted, deletedAtUtc);
}