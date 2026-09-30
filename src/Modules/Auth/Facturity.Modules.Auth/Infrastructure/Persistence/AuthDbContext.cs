using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Facturity.Modules.Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Auth.Infrastructure;

internal class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    
    internal DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("auth");
        
        var tenantColumn = modelBuilder.Entity<ApplicationUser>();
            tenantColumn.HasIndex(u => u.TenantId);
            tenantColumn.Property(u => u.TenantId)
                .HasColumnName("tenant_id")
                .ValueGeneratedNever();
            
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}