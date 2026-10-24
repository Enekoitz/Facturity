using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Auth.Domain;

internal static class AuthErrors
{
    internal static readonly Error InvalidLogin =
        Error.Validation("Auth.InvalidLogin", "Invalid email or password.");
    
    internal static readonly Error InvalidRefreshToken =
        Error.Unauthorized("Auth.InvalidRefreshToken", "Invalid refresh token.");
}