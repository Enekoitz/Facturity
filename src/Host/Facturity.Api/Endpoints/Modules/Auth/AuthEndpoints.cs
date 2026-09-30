using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Facturity.Api.Auth;
using Facturity.Api.Helpers;
using Facturity.Modules.Auth.Features;
using Facturity.Modules.Customer.Domain;
using Microsoft.Extensions.Options;

namespace Facturity.Api.Endpoints.Modules.Auth;

public static class AuthEndpoints
{
    private static readonly Guid TenantIdProvisional = Guid.Parse("2b4f5701-30be-4eb4-b30e-2fa5f4a6a128");
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapPost("/refresh", RefreshAsync);
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
        LoginHandler loginHandler,
        ITokenService tokenService,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var loginResult = await loginHandler.HandleAsync(command, ct);

        if (loginResult.IsFailure)
            return loginResult.ToProblemDetails(httpContext);

        var tokenString = tokenService.GenerateAccessToken(
            loginResult.Value.UserId, 
            loginResult.Value.Email,
            loginResult.Value.TenantId);
        
        httpContext.AddRefreshTokenAsCookieAtResponse(
            loginResult.Value.RefreshToken, 
            refreshTokenOptions.Value.ExpirationDays,
            AuthConstants.RefreshTokenCookiePath);

        return Results.Ok(new LoginResponse(tokenString));
    }

    private static async Task<IResult> RefreshAsync(
        RefreshHandler handler,
        ITokenService tokenService,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (!httpContext.Request.Cookies.TryGetValue(AuthConstants.RefreshTokenCookieName, out var refreshToken))
            return Results.Unauthorized();
        
        var command = new RefreshTokenCommand(refreshToken);
        
        var result = await handler.HandleAsync(command, ct);

        if (result.IsFailure)
            return result.ToProblemDetails(httpContext);
        
        httpContext.AddRefreshTokenAsCookieAtResponse(
            result.Value.RefreshToken, 
            refreshTokenOptions.Value.ExpirationDays, 
            AuthConstants.RefreshTokenCookiePath );

        var accessToken = tokenService.GenerateAccessToken(
            result.Value.UserId, 
            result.Value.Email, 
            TenantIdProvisional);

        return Results.Ok(new LoginResponse(accessToken));
    }

    private static IResult GetMe(ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(JwtRegisteredClaimNames.Email);
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var tenantId = user.FindFirstValue(CustomClaimTypes.TenantId);

        return Results.Ok(new { id = sub, email, tenantId });
    }
}