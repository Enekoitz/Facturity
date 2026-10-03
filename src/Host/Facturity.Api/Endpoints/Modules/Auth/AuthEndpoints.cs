using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Facturity.Api.Auth;
using Facturity.Api.Helpers;
using Facturity.Modules.Auth.Features;
using Facturity.Modules.SharedKernel.Abstractions;
using Facturity.Modules.SharedKernel.Domain;
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
        group.MapPost("/logout", LogoutAsync);
        group.MapGet("/me", GetMe).RequireAuthorization();
    }
    
    private static async Task<IResult> RegisterAsync(
        RegisterCommand command,
        ICommandHandler<RegisterCommand, Result<RegisterResult>> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.Created($"/api/auth/{result.Value.UserId}", result.Value)
            : result.ToProblemDetails(httpContext);
    }

    private record LoginResponse(string AccessToken);
    
    private static async Task<IResult> LoginAsync(
        LoginCommand command,
        ICommandHandler<LoginCommand, Result<LoginResult>>  loginHandler,
        ITokenService tokenService,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var result = await loginHandler.HandleAsync(command, ct);
        if (result.IsFailure)
            return result.ToProblemDetails(httpContext);

        var tokenString = tokenService.GenerateAccessToken(
            result.Value.UserId, 
            result.Value.Email,
            result.Value.TenantId);
        
        httpContext.AddRefreshTokenCookie(
            result.Value.RefreshToken, 
            refreshTokenOptions.Value.ExpirationDays,
            AuthConstants.RefreshTokenCookiePath);

        return Results.Ok(new LoginResponse(tokenString));
    }

    private static async Task<IResult> RefreshAsync(
        ICommandHandler<RefreshCommand, Result<RefreshResult>> handler,
        ITokenService tokenService,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (!httpContext.Request.Cookies.TryGetValue(AuthConstants.RefreshTokenCookieName, out var refreshToken))
            return Results.Unauthorized();
        
        var command = new RefreshCommand(refreshToken);
        
        var result = await handler.HandleAsync(command, ct);
        if (result.IsFailure)
            return result.ToProblemDetails(httpContext);
        
        httpContext.AddRefreshTokenCookie(
            result.Value.RefreshToken, 
            refreshTokenOptions.Value.ExpirationDays, 
            AuthConstants.RefreshTokenCookiePath );

        var accessToken = tokenService.GenerateAccessToken(
            result.Value.UserId, 
            result.Value.Email, 
            result.Value.TenantId);

        return Results.Ok(new LoginResponse(accessToken));
    }
    
    private static async Task<IResult> LogoutAsync(
        ICommandHandler<LogoutCommand, Result> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        httpContext.Request.Cookies.TryGetValue(AuthConstants.RefreshTokenCookieName, out var refreshToken);
        
        httpContext.DeleteRefreshTokenCookie(AuthConstants.RefreshTokenCookiePath );
        
        if (refreshToken == null) 
            return Results.Ok();
        
        var command = new LogoutCommand(refreshToken);
        var result = await handler.HandleAsync(command, ct);

        return result.IsFailure 
            ? result.ToProblemDetails(httpContext)
            : Results.Ok();
    }

    private static IResult GetMe(ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(JwtRegisteredClaimNames.Email);
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var tenantId = user.FindFirstValue(CustomClaimTypes.TenantId);

        return Results.Ok(new { id = sub, email, tenantId });
    }
}