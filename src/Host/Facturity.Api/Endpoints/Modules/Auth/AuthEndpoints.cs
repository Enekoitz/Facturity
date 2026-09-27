using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Facturity.Api.Auth;
using Facturity.Api.Helpers;
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
        group.MapGet("/me", GetMe).RequireAuthorization();
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

        var tokenString = tokenService.GenerateAccessToken(loginResult.Value.UserId, loginResult.Value.Email);

        return Results.Ok(new LoginResponse(tokenString));
    }

    private static IResult GetMe(ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return Results.Ok(new { id = sub, email });
    }
}