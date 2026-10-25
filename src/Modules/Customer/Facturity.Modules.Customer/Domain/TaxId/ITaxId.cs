namespace Facturity.Modules.Customer.Domain;

public interface ITaxId
{
    bool? IsLegalEntity { get; }
}