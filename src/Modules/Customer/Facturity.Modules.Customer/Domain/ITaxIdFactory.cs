using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public interface ITaxIdFactory
{
    Result<ITaxId> Create(
        string value,
        TaxIdType type,
        string? countryCode);
}