using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Facturity.Api.Auth;
using Facturity.Modules.Auth.Features;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Facturity.IntegrationTests.Auth;

public class AuthHappyPathTests(AuthApiFactory factory) : IClassFixture<AuthApiFactory>
{
    private static readonly Uri Https = new("https://localhost");

    private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        { BaseAddress = Https, HandleCookies = true, AllowAutoRedirect = false });

    private readonly HttpClient _replayClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        { BaseAddress = Https, HandleCookies = false, AllowAutoRedirect = false });

    private record TokenResponse(string AccessToken);
    
    private const string LoginUrl = "/api/auth/login";
    private const string RegisterUrl = "/api/auth/register";
    private const string RefreshUrl = "/api/auth/refresh";
    private const string GetMeUrl = "/api/auth/me";
    private const string LogoutUrl = "/api/auth/logout";

    [Fact]
    public async Task Flujo_completo_registro_login_refresh_logout()
    {
        var email = $"{Guid.NewGuid():N}@test.eus";
        const string password = "P@assW0rd_2000_factu_riTY-134(%)";
        const string userName = "Garry_27";

        // 1. Registro. Se registra correctamente el usuario en el sistema
        var credRegister = new RegisterCommand(userName, email, password);
        var register = await _client.PostAsJsonAsync(RegisterUrl,  credRegister );
        Assert.True(register.IsSuccessStatusCode);

        // 2. Login. El usuario registrado inicia sesión (recibe access token en el response body y el refresh token como cookie 'refreshToken')
        var credentials = new LoginCommand(email, password);
        var login = await _client.PostAsJsonAsync(LoginUrl, credentials);
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var access1 = (await login.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
        Assert.False(string.IsNullOrEmpty(access1));
        var cookie1 = RefreshCookie(login);
        Assert.Contains("httponly", cookie1, StringComparison.OrdinalIgnoreCase);

        // 3. Endpoint protegido con el access token
        var me = await Send(HttpMethod.Get, GetMeUrl, access1);
        Assert.Equal(HttpStatusCode.OK, me.StatusCode);

        // 4. Refresh: el cliente reenvía únicamente la cookie con el refresh token; recibe refresh token y access token nuevos
        var refresh = await _client.PostAsync(RefreshUrl, null);
        Assert.Equal(HttpStatusCode.OK, refresh.StatusCode);
        var access2 = (await refresh.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
        var cookie2 = RefreshCookie(refresh);
        Assert.NotEqual(cookie1.Split(';')[0], cookie2.Split(';')[0]); // rotación

        // 5. Logout: el cliente cierra sesión enviando access token y refresh token
        var logout = await Send(HttpMethod.Post, LogoutUrl, access2);
        Assert.True(logout.IsSuccessStatusCode);

        // 6a. Con el cliente normal: la cookie ya no está
        var after = await _client.PostAsync(RefreshUrl, null);
        Assert.Equal(HttpStatusCode.Unauthorized, after.StatusCode);

        // 6b. Reenvío a mano de la cookie anterior: ¿lo revocó el servidor?
        var replay = new HttpRequestMessage(HttpMethod.Post, RefreshUrl);
        replay.Headers.Add("Cookie", cookie2.Split(';')[0]);
        var replayed = await _replayClient.SendAsync(replay);
        Assert.Equal(HttpStatusCode.Unauthorized, replayed.StatusCode);
    }

    private static string RefreshCookie(HttpResponseMessage r) =>
        r.Headers.GetValues("Set-Cookie").Single(c => c.StartsWith($"{AuthConstants.RefreshTokenCookieName}="));

    private Task<HttpResponseMessage> Send(HttpMethod method, string url, string accessToken)
    {
        var req = new HttpRequestMessage(method, url);
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return _client.SendAsync(req);
    }
}