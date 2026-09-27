using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Facturity.Api.Auth;

public static class JwtExtension
{
    public static void AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Mapeo y validación de JwtSettings con IOptions
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.SigningKey) && options.SigningKey.Length >= 32,
                "Jwt:SigningKey es obligatoria y debe tener al menos 32 caracteres.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "Jwt:Issuer no puede estar vacío.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "Jwt:Audience no puede estar vacío.")
            .Validate(options => options.ExpirationMinutes > 0, "Jwt:ExpirationMinutes debe ser mayor a 0.")
            .ValidateOnStart();

        // 2. Extraer valores necesarios para el Middleware de Auth
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("No se encontró la sección 'Jwt' en la configuración.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));

        // 3. Configuración del esquema de JwtBearer
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();
        
        services.AddScoped<ITokenService, JwtTokenService>();
    }
}