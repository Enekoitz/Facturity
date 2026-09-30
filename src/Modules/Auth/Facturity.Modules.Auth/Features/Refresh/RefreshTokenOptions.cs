namespace Facturity.Modules.Auth.Features;

internal class RefreshTokenOptions
{
    internal const string SectionName = "RefreshToken";
    internal int ExpirationDays { get; init; } = 21;
}