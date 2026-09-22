namespace Facturity.Modules.SharedKernel.Domain;

public abstract class AggregateRoot<TId> : ITenantEntity
{
    public TId Id { get; protected set; }
    public TenantId TenantId { get; private set; }

    protected AggregateRoot() { }

    protected AggregateRoot(TenantId tenantId)
    {
        //if (tenantId.Value <= 0)
        //    throw new ArgumentException("El TenantId no es válido.", nameof(tenantId));

        TenantId = tenantId;
    }

    protected AggregateRoot(TId id, TenantId tenantId)
    {
        //if (tenantId.Value <= 0)
        //    throw new ArgumentException("El TenantId no es válido.", nameof(tenantId));
        Id = id;
        TenantId = tenantId;
    }
}