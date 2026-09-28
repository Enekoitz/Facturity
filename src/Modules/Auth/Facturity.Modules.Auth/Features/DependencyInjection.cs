using Microsoft.Extensions.DependencyInjection;

namespace Facturity.Modules.Auth.Features;

public static class DependencyInjection
{
    public static void AddAuthFeatures(this IServiceCollection services)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<RefreshTokenIssuer>();
        services.AddScoped<RefreshTokenHandler>();
    }
}