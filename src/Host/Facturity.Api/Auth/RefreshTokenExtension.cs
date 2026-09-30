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

    extension(HttpContext httpContext)
    {
        public void AddRefreshTokenCookie(string refreshToken, int expirationDays, string uri)
        {
            httpContext.Response.Cookies.Append(RefreshTokenOptions.SectionName, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(expirationDays),
                Path = uri
            });
        }

        public void DeleteRefreshTokenCookie(string uri)
        {
            httpContext.Response.Cookies.Delete(RefreshTokenOptions.SectionName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = uri
            });
        }
    }
}