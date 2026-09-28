using Facturity.Modules.Auth.Features;

namespace Facturity.Api.Auth;

public static class RefreshTokenExtension
{
    public static void AddRefreshTokenSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RefreshTokenOptions>(
            configuration.GetSection(RefreshTokenOptions.SectionName)
            );
    }

    public static void AddRefreshTokenAsCookieAtResponse(this HttpContext context, string refreshToken, int expirationDays, string uri)
    {
        context.Response.Cookies.Append(RefreshTokenOptions.SectionName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(expirationDays),
            Path = uri
        });
    }
}