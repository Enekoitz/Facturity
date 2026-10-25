using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Facturity.Modules.Customer.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomerFeatures(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateCustomerCommand, Result<Guid>>, CreateCustomerHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerCommand, Result>, UpdateCustomerHandler>();
        services.AddScoped<IQueryHandler<GetCustomersQuery, Result<IEnumerable<Domain.Customer>>>, GetCustomersHandler>();
        services.AddScoped<ICommandHandler<DeleteCustomerCommand, Result>, DeleteCustomerHandler>();
        services.AddScoped<IQueryHandler<GetByIdCustomerQuery, Result<GetByIdCustomerDto>>, GetByIdCustomerHandler>();
        services.AddScoped<ICommandHandler<DeactivateCustomerCommand, Result>, DeactivateCustomerHandler>();
        services.AddScoped<ICommandHandler<ActivateCustomerCommand, Result>, ActivateCustomerHandler>();
        return services;
    }
}