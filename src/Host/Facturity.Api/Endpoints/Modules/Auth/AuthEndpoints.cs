using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facturity.Api.Settings;
using Facturity.Modules.Auth.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Facturity.Modules.Auth.Features;

namespace Facturity.Api.Endpoints.Modules.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
    }
    
    private static async Task<IResult> RegisterAsync(
        RegisterCommand command,
        RegisterHandler handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var registerResult = await handler.HandleAsync(command, ct);

        return registerResult.IsSuccess
            ? Results.Created($"/auth/{registerResult.Value.UserId}", registerResult.Value)
            : registerResult.ToProblemDetails(httpContext);
    }

        return result.Succeeded
            ? Results.Created($"/auth/{user.Id}", new { user.Id, user.Email })
            : Results.ValidationProblem(
                result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtOptions)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return Results.Unauthorized();

        var jwt = jwtOptions.Value;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.ExpirationMinutes),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(new { token = tokenString });
    }
}

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);