namespace Facturity.Modules.Auth.Domain;

internal class RefreshToken
{
    internal Guid Id { get; set; } = Guid.NewGuid();
    internal Guid UserId { get; set; }
    internal Guid FamilyId { get; set; }
    internal string TokenHash { get; set; } = string.Empty;
    internal DateTime ExpiresAt { get; set; }
    internal DateTime CreatedAt { get; set; }
    internal DateTime? RevokedAt { get; set; }
}