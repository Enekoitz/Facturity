namespace Facturity.Modules.SharedKernel.Domain;

public readonly record struct TenantId
{
    public Guid Value { get; }

    private TenantId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TenantId must be a non-empty Guid value.", nameof(value));

        Value = value;
    }

    public static TenantId From(Guid value) => new(value);

    public static Result<TenantId> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Result.Fail<TenantId>(Error.Validation("Tenant.InvalidTenantID", "Invalid TenantId."));

        return Result.Ok(new TenantId(value));
    }

    public static TenantId Parse(string value)
    {
        if (!Guid.TryParse(value, out var id))
            throw new InvalidOperationException("Invalid TenantId.");

        return new TenantId(id);
    }

    public override string ToString() => Value.ToString();

    public static explicit operator Guid(TenantId tenantId) => tenantId.Value;
}