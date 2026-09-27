using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Auth.Domain;

public static class AuthErrors
{
    internal static readonly Error InvalidLogin =
        Error.Validation("Auth.InvalidLogin", "That email and password combination is incorrect.");
}