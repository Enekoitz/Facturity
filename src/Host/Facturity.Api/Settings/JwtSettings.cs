namespace Facturity.Api.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public required string Issuer { get; init; } = string.Empty;
    public required string Audience { get; init; } = string.Empty;
    public required string SigningKey { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 15;
}