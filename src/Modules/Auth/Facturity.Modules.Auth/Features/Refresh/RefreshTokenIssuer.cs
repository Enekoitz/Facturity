using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using Facturity.Modules.Auth.Domain;
using Facturity.Modules.Auth.Infrastructure;
using Microsoft.Extensions.Options;

namespace Facturity.Modules.Auth.Features;

internal class RefreshTokenIssuer(
    AuthDbContext dbContext, 
    IOptions<RefreshTokenOptions> options,
    TimeProvider timeProvider)
{
    internal async Task<string> IssueAsync(Guid userId, Guid familyId, CancellationToken ct)
    {
        var rawBase64 = Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(64));
        var utcNow = timeProvider.GetUtcNow();
        
        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            FamilyId = familyId,
            TokenHash = ToHashSha256(rawBase64),
            ExpiresAt = utcNow.AddDays(options.Value.ExpirationDays),
            CreatedAt = utcNow
        });

        await dbContext.SaveChangesAsync(ct);
        return rawBase64;
    }

    internal static string ToHashSha256(string raw) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
}