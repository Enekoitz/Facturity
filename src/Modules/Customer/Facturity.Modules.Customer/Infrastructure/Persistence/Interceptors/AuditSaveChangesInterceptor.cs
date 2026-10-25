using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Facturity.Modules.Customer.Infrastructure;

public class AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAudit(DbContext? context)
    {
        if (context is null) return;

        // TODO: Pending change to user identification via JWT—should it be email, username, or GUID?
        
        var user = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Metadata.FindProperty("created_at_utc") is null)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("created_at_utc").CurrentValue = utcNow;
                    entry.Property("created_by").CurrentValue = user;
                    break;
                case EntityState.Modified:
                    entry.Property("updated_at_utc").CurrentValue = utcNow;
                    entry.Property("updated_by").CurrentValue = user;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}