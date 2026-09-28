using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Auth.Domain;

public static class AuthErrors
{
    internal static readonly Error InvalidLogin =
        Error.Validation("Auth.InvalidLogin", "That email and password combination is incorrect.");
    
    internal static readonly Error InvalidRefreshToken =
        Error.Unauthorized("Auth.InvalidRefreshToken", "El refresh token no es válido.");
}