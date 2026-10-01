using Facturity.Modules.Auth.Domain;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
using Microsoft.AspNetCore.Identity;

namespace Facturity.Modules.Auth.Features;

internal record RegisterCommand(string UserName, string Email, string Password) : ICommand<Result<RegisterResult>>;

internal record RegisterResult(Guid UserId, string Email, string UserName);

internal sealed class RegisterHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<RegisterCommand, Result<RegisterResult>>
{
    public async Task<Result<RegisterResult>> HandleAsync(RegisterCommand command, CancellationToken ct)
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
            TenantId = Guid.Parse("2b4f5701-30be-4eb4-b30e-2fa5f4a6a128")
        };

        if (existing is not null)
            return Result.Ok(new RegisterResult(user.Id, user.Email, user.UserName));
            
        var identityResult = await userManager.CreateAsync(user, command.Password);
        if (identityResult.Succeeded) 
            return Result.Ok(new RegisterResult(user.Id, user.Email, user.UserName));
        
        errors.AddRange(identityResult.Errors.Select(error => Error.Validation($"Auth.{error.Code}", error.Description)));
        return Result.Fail<RegisterResult>(errors);
    } 
}