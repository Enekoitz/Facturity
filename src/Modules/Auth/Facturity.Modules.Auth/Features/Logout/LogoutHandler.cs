using Facturity.Modules.Auth.Infrastructure;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore;

namespace Facturity.Modules.Auth.Features;

internal record LogoutCommand(string RefreshToken) : ICommand<Result>;

internal sealed class LogoutHandler(
    AuthDbContext dbContext,
    TimeProvider timeProvider)
        :  ICommandHandler<LogoutCommand, Result>
{
    public async Task<Result> HandleAsync(
        LogoutCommand command,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
            return Result.Ok();

        var hash = RefreshTokenIssuer.ToHashSha256(command.RefreshToken);
        
        var stored = await dbContext.RefreshTokens.SingleOrDefaultAsync(t => t.TokenHash == hash, ct);
        if (stored is null)
            return Result.Ok();

        var utcNow = timeProvider.GetUtcNow();

        await dbContext.RefreshTokens
            .Where(t => t.FamilyId == stored.FamilyId && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAt, utcNow), ct);

        return Result.Ok();
    } 
}
