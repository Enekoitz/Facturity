namespace Facturity.Modules.SharedKernel.Domain;

public readonly record struct TenantId
{
    public int Value { get; }

    private TenantId(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El TenantId debe ser un entero positivo.", nameof(value));

        Value = value;
    }

    public static TenantId From(int value) => new(value);

    public static Result<TenantId> Crear(int value)
    {
        if (value <= 0)
            return Result.Fail<TenantId>(Error.Validation("TenantIdInvalido", "TenantId inválido."));

        return Result.Ok(new TenantId(value));
    }

    public static TenantId Parse(string value)
    {
        if (!int.TryParse(value, out int id))
            throw new InvalidOperationException("TenantId inválido.");

        return new TenantId(id);
    }

    public override string ToString() => Value.ToString();

    public static explicit operator int(TenantId tenantId) => tenantId.Value;
}