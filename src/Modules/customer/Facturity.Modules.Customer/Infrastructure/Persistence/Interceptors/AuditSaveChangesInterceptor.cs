using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Facturity.Modules.Customer.Infrastructure;

public class AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        AplicarAuditoria(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AplicarAuditoria(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AplicarAuditoria(DbContext? context)
    {
        if (context is null) return;

        // TODO: Pendiente de cambiar por usuario por JWT ¿email, user, guid?
        
        var usuario = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
        var ahora = DateTimeOffset.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            // Si la entidad no tiene la shadow property, no es auditable: se ignora.
            if (entry.Metadata.FindProperty("created_at_utc") is null)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("created_at_utc").CurrentValue = ahora;
                    entry.Property("created_by").CurrentValue = usuario;
                    break;
                case EntityState.Modified:
                    entry.Property("updated_at_utc").CurrentValue = ahora;
                    entry.Property("updated_by").CurrentValue = usuario;
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