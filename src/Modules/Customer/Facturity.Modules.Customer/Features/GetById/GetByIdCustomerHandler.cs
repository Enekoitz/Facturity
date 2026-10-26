using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed class GetByIdCustomerHandler(
    ICustomerRepository repo) 
    : IQueryHandler<GetByIdCustomerQuery, Result<GetByIdCustomerDto>>
{
    public async Task<Result<GetByIdCustomerDto>> HandleAsync(GetByIdCustomerQuery request, CancellationToken ct)
    {
        var customer = await repo.GetByIdAsync(request.Id, ct);

        return customer is null 
            ? Result<GetByIdCustomerDto>.Fail(CustomerErrors.NotFound) 
            : Result<GetByIdCustomerDto>.Ok(customer.ToDto());
    }
}
