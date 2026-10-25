using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class GetCustomersHandler(ICustomerRepository repo) 
    : IQueryHandler<GetCustomersQuery, Result<IEnumerable<Domain.Customer>>>
{
    public async Task<Result<IEnumerable<Domain.Customer>>> HandleAsync(GetCustomersQuery query, CancellationToken ct = default)
    {
        var customers = await repo.GetAsync(query, ct);
        return Result.Ok(customers);
    }
}
