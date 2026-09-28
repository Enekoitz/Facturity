using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Facturity.Api.Auth;
using Facturity.Api.Helpers;
using Facturity.Modules.Auth.Features;

namespace Facturity.Api.Endpoints.Modules.Auth;

public static class AuthEndpoints
{
    private static readonly Guid TenantIdProvisional = Guid.Parse("2b4f5701-30be-4eb4-b30e-2fa5f4a6a128");
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapGet("/me", GetMe).RequireAuthorization();
    }
    
    private static async Task<IResult> RegisterAsync(
        RegisterCommand command,
        RegisterHandler handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var fullCommand = command with{TenantId = TenantIdProvisional};
        var registerResult = await handler.HandleAsync(fullCommand, ct);

        return registerResult.IsSuccess
            ? Results.Created($"/auth/{registerResult.Value.UserId}", registerResult.Value)
            : registerResult.ToProblemDetails(httpContext);
    }

    private record LoginResponse(string AccessToken);
    
    private static async Task<IResult> LoginAsync(
        LoginCommand command,
        LoginHandler handler,
        ITokenService tokenService,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var loginResult = await handler.HandleAsync(command, ct);

        if (loginResult.IsFailure)
            return loginResult.ToProblemDetails(httpContext);

        var tokenString = tokenService.GenerateJwtAccessToken(
            loginResult.Value.UserId, 
            loginResult.Value.Email,
            loginResult.Value.TenantId);

        return Results.Ok(new LoginResponse(tokenString));
    }

    private static IResult GetMe(ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(JwtRegisteredClaimNames.Email);
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var tenantId = user.FindFirstValue(CustomClaimTypes.TenantId);

        return Results.Ok(new { id = sub, email, tenantId });
    }
}