using System.IdentityModel.Tokens.Jwt;

namespace Facturity.Api.Auth;

public static class CustomClaimTypes
{
    public const string TenantId = "tenant_id";
    public const string UserType = "user_type";
    public const string Role = "role";
    public const string Permission = "permission";
}