using Facturity.Modules.Auth.Domain;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;

namespace Facturity.Modules.Auth.Features;

internal record LoginCommand(string Email, string Password);

internal record LoginResult(Guid UserId, string Email, Guid TenantId, string RefreshToken);

internal sealed class LoginHandler(UserManager<ApplicationUser> userManager, RefreshTokenIssuer issuer)
{
    internal async Task<Result<LoginResult>> HandleAsync(
        LoginCommand command,
        CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, command.Password))
            return Result.Fail<LoginResult>(Error.Unauthorized(AuthErrors.InvalidLogin.Code, AuthErrors.InvalidLogin.Description));
        
        var refreshToken = await issuer.IssueAsync(user.Id, familyId: Guid.NewGuid(), ct);
        
        return Result.Ok(new LoginResult(user.Id, user.Email!, user.TenantId, refreshToken));
    } 
}