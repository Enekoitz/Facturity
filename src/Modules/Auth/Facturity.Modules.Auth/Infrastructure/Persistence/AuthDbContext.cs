using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Facturity.Modules.Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Auth.Infrastructure;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("auth");
        base.OnModelCreating(modelBuilder);
    }
}