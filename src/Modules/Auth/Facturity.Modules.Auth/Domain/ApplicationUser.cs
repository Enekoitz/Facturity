using Microsoft.AspNetCore.Identity;

namespace Facturity.Modules.Auth.Domain;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public Guid TenantId { get; init; }
}