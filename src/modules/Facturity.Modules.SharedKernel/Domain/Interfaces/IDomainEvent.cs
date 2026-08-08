namespace Facturity.Modules.SharedKernel.Domain
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OcurridoEn { get; }
    }
}
