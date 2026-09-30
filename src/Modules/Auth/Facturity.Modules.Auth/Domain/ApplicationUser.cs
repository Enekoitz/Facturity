using Microsoft.AspNetCore.Identity;

namespace Facturity.Modules.Auth.Domain;

internal sealed class ApplicationUser : IdentityUser<Guid>
{
    internal Guid TenantId { get; init; }
}