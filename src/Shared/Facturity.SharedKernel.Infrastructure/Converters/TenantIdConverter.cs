using Facturity.Modules.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Facturity.Modules.SharedKernel.Infrastructure;

public sealed class TenantIdConverter()
    : ValueConverter<TenantId, Guid>(id => id.Value, value => TenantId.Create(value).Value);