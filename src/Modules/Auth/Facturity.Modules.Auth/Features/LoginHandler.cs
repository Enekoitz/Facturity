using Facturity.Modules.Auth.Domain;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;

namespace Facturity.Modules.Auth.Features;

internal sealed class LoginHandler(UserManager<ApplicationUser> userManager)
{
    internal async Task<Result<LoginUserDto>> HandleAsync(LoginCommand command, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, command.Password))
            return Result.Fail<LoginUserDto>(Error.Unauthorized(AuthErrors.InvalidLogin.Code, AuthErrors.InvalidLogin.Description));
        
        return Result.Ok(new LoginUserDto(user.Id, user.Email!, user.TenantId));
    } 
}

internal record LoginCommand(string Email, string Password);

internal record LoginUserDto(Guid UserId, string Email, Guid TenantId);