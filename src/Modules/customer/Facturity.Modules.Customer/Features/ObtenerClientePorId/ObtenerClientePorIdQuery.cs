using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record ObtenerClientePorIdQuery(Guid Id) : IQuery<Result<ClienteDto>>;
