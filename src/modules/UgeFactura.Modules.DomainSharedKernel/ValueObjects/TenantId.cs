namespace UgeFactura.Modules.DomainSharedKernel;

public enum TenantErrorCode
{
    EmptyGuid
}

public sealed class TenantId : ValueObject
{
    public Guid Value { get; }

    private TenantId(Guid value)
    {
        Value = value;
    }

    public static Result<TenantId> Crear(Guid value)
    {
        if (value == Guid.Empty)
            return Result.Fail<TenantId>(Error.Validation(TenantErrorCode.EmptyGuid.ToString(), "TenantId inválido."));

        TenantId tenantId = new(value);
        return Result.Ok(tenantId);
    }

    public static TenantId Crear(string value)
    {
        if (!Guid.TryParse(value, out var guid) || guid == Guid.Empty)
            throw new InvalidOperationException("TenantId inválido.");

        return new TenantId(guid);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
