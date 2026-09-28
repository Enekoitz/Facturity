using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using Facturity.Modules.Auth.Domain;
using Facturity.Modules.Auth.Infrastructure;
using Microsoft.Extensions.Options;

namespace Facturity.Modules.Auth.Features;

public class RefreshTokenIssuer(AuthDbContext db, IOptions<RefreshTokenOptions> options)
{
    public async Task<string> IssueAsync(Guid userId, Guid familyId, CancellationToken ct)
    {
        var rawBase64 = Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(64));
        var urcNow = DateTime.UtcNow;

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            FamilyId = familyId,
            TokenHash = ToHashSha256(rawBase64),
            ExpiresAt = urcNow.AddDays(options.Value.ExpirationDays),
            CreatedAt = urcNow
        });

        await db.SaveChangesAsync(ct);
        return rawBase64;
    }

    public static string ToHashSha256(string raw) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
}