using Facturity.Modules.Auth.Domain;
using Facturity.Modules.Auth.Infrastructure;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Auth.Features;

internal record RefreshCommand(string RefreshToken): ICommand<Result<RefreshResult>>;
internal record RefreshResult(Guid UserId, string Email, Guid TenantId, string RefreshToken);

internal class RefreshHandler(
    AuthDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RefreshTokenIssuer issuer,
    TimeProvider timeProvider)
: ICommandHandler<RefreshCommand, Result<RefreshResult>>
{
    private static Result<RefreshResult> IsInvalid =>
        Result<RefreshResult>.Fail(AuthErrors.InvalidRefreshToken);
    
    public async Task<Result<RefreshResult>> HandleAsync(RefreshCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
            return IsInvalid;

        var hash = RefreshTokenIssuer.ToHashSha256(command.RefreshToken);
        
        // Debe existir el refresh token en el sistema, de lo contrario la operación es inválida
        var stored = await dbContext.RefreshTokens.SingleOrDefaultAsync(t => t.TokenHash == hash, ct);
        if (stored is null)
            return IsInvalid;

        var utcNow = timeProvider.GetUtcNow();

        // Token ya usado/revocado que reaparece: posible robo, así que se invalida toda la sesión
        if (stored.RevokedAt is not null)
        {
            await dbContext.RefreshTokens
                .Where(t => t.FamilyId == stored.FamilyId && t.RevokedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, utcNow), ct);
            return IsInvalid;
        }

        if (stored.ExpiresAt <= utcNow)
            return IsInvalid;

        var user = await userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null || await userManager.IsLockedOutAsync(user))
            return IsInvalid;

        // Revocación atómica: si llegan dos peticiones a la vez con el mismo token, solo una gana
        var revoked = await dbContext.RefreshTokens
            .Where(t => t.Id == stored.Id && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, utcNow), ct);
        if (revoked == 0)
            return IsInvalid;

        var newRefreshToken = await issuer.IssueAsync(user.Id, stored.FamilyId, ct);

        return Result<RefreshResult>.Ok(
            new RefreshResult(user.Id, user.Email!, user.TenantId, newRefreshToken));
    }
}