namespace Facturity.Modules.Auth.Features;

public class RefreshTokenOptions
{
    public const string SectionName = "RefreshToken";
    public int ExpirationDays { get; init; } = 21;
}