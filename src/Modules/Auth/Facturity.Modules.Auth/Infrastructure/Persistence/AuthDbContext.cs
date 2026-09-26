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
        
        var tenantColumn = modelBuilder.Entity<ApplicationUser>();
            tenantColumn.HasIndex(u => u.TenantId);
            tenantColumn.Property(u => u.TenantId)
                .HasColumnName("tenant_id")
                .ValueGeneratedNever();
        
        base.OnModelCreating(modelBuilder);
    }
}