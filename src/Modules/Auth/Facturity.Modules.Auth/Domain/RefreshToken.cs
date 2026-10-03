namespace Facturity.Modules.Auth.Domain;

internal class RefreshToken
{
    internal Guid Id { get; set; } = Guid.NewGuid();
    internal Guid UserId { get; set; }
    internal Guid FamilyId { get; set; }
    internal string TokenHash { get; set; } = string.Empty;
    internal DateTimeOffset ExpiresAt { get; set; }
    internal DateTimeOffset CreatedAt { get; set; }
    internal DateTimeOffset? RevokedAt { get; set; }
}