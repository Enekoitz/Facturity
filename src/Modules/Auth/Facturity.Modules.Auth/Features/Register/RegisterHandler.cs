using Facturity.Modules.Auth.Domain;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;

namespace Facturity.Modules.Auth.Features;

internal sealed class RegisterHandler(UserManager<ApplicationUser> userManager)
{
    internal async Task<Result<RegisteredUserDto>> HandleAsync(RegisterCommand command, CancellationToken ct)
    {
        var errors = new List<Error>();
        
        // nunca hay que mandar al cliente información de si un email está ya en uso o no.
        // mejor hacer lo siguiente: comunicar "Revisa tu correo para confirmar la cuenta."
        // - si el correo existe se envía email de seguridad tipo "Intento de registro con su correo"
        // - si el correo no existe se envía email para confirmar cuenta y dar bienvenida por ejemplo.
        
        var existing = await userManager.FindByEmailAsync(command.Email) 
                       ?? await userManager.FindByNameAsync(command.UserName);

        // if (existing is not null)
        // errors.Add(AuthErrors.EmailInUse);
        var user = new ApplicationUser
        {
            UserName = command.UserName,
            Email = command.Email,
            TenantId = command.TenantId
        };

        if (existing is not null)
            return Result.Ok(new RegisteredUserDto(user.Id, user.Email, user.UserName));
            
        var identityResult = await userManager.CreateAsync(user, command.Password);
        if (identityResult.Succeeded) 
            return Result.Ok(new RegisteredUserDto(user.Id, user.Email, user.UserName));
        
        errors.AddRange(identityResult.Errors.Select(error => Error.Validation($"Auth.{error.Code}", error.Description)));
        return Result.Fail<RegisteredUserDto>(errors);
    } 
}

internal record RegisterCommand(string UserName, string Email, string Password, Guid TenantId);

internal record RegisteredUserDto(Guid UserId, string Email, string UserName);