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
    public async Task Complete_flow_for_registration_login_refresh_and_logout()
    {
        var email = $"{Guid.NewGuid():N}@test.eus";
        const string password = "P@assW0rd_2000_factu_riTY-134(%)";
        const string userName = "fake_user_23&__";

        // Registration: The user is successfully registered in the system.
        var credRegister = new RegisterCommand(userName, email, password);
        var register = await _client.PostAsJsonAsync(RegisterUrl,  credRegister );
        Assert.True(register.IsSuccessStatusCode);

        // Login: The registered user logs in (receives an access token in the response body and
        // a refresh token as a cookie named 'refreshToken')
        var credentials = new LoginCommand(email, password);
        var login = await _client.PostAsJsonAsync(LoginUrl, credentials);
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var access1 = (await login.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
        Assert.False(string.IsNullOrEmpty(access1));
        var cookie1 = RefreshCookie(login);
        Assert.Contains("httponly", cookie1, StringComparison.OrdinalIgnoreCase);

        // Endpoint protected with the access token
        var me = await Send(HttpMethod.Get, GetMeUrl, access1);
        Assert.Equal(HttpStatusCode.OK, me.StatusCode);

        // Refresh: The client sends back only the cookie containing the refresh token;
        // it receives a new refresh token and a new access token
        var refresh = await _client.PostAsync(RefreshUrl, null);
        Assert.Equal(HttpStatusCode.OK, refresh.StatusCode);
        var access2 = (await refresh.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
        var cookie2 = RefreshCookie(refresh);
        Assert.NotEqual(cookie1.Split(';')[0], cookie2.Split(';')[0]); // rotación

        // Logout: The client logs out by sending the access token and refresh token.
        var logout = await Send(HttpMethod.Post, LogoutUrl, access2);
        Assert.True(logout.IsSuccessStatusCode);

        // With the standard client: the cookie is no longer there
        var after = await _client.PostAsync(RefreshUrl, null);
        Assert.Equal(HttpStatusCode.Unauthorized, after.StatusCode);

        // Manual resubmission of the previous cookie. The server has successfully revoked it.
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