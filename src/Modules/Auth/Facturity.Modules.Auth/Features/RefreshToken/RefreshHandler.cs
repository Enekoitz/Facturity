using Facturity.Modules.Auth.Domain;
using Facturity.Modules.Auth.Infrastructure;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Auth.Features;


public record RefreshTokenCommand(string RefreshToken);
public record RefreshTokenResult(Guid UserId, string Email, string RefreshToken);

public class RefreshTokenHandler(
    AuthDbContext db,
    UserManager<ApplicationUser> userManager,
    RefreshTokenIssuer issuer)
{
    private static Result<RefreshTokenResult> IsInvalid =>
        Result<RefreshTokenResult>.Fail(AuthErrors.InvalidRefreshToken);
    
    public async Task<Result<RefreshTokenResult>> HandleAsync(RefreshTokenCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
            return IsInvalid;

        var hash = RefreshTokenIssuer.ToHashSha256(command.RefreshToken);
        
        // Debe existir el refresh token en el sistema, de lo contrario la operación es inválida
        var stored = await db.RefreshTokens.SingleOrDefaultAsync(t => t.TokenHash == hash, ct);
        if (stored is null)
            return IsInvalid;

        var now = DateTime.UtcNow;

        // Casuística A.
        // Token ya usado/revocado que reaparece: posible robo, así que se invalida toda la sesión
        if (stored.RevokedAt is not null)
        {
            await db.RefreshTokens
                .Where(t => t.FamilyId == stored.FamilyId && t.RevokedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, now), ct);
            return IsInvalid;
        }

        if (stored.ExpiresAt <= now)
            return IsInvalid;

        var user = await userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null || await userManager.IsLockedOutAsync(user))
            return IsInvalid;

        // Revocación atómica: si llegan dos peticiones a la vez con el mismo token, solo una gana
        var revoked = await db.RefreshTokens
            .Where(t => t.Id == stored.Id && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, now), ct);
        if (revoked == 0)
            return IsInvalid;

        var newRefreshToken = await issuer.IssueAsync(user.Id, stored.FamilyId, ct);

        return Result<RefreshTokenResult>.Ok(
            new RefreshTokenResult(user.Id, user.Email!, newRefreshToken));
    }
}