using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Facturity.IntegrationTests.Auth.Security;

public class EndpointProtectionTests(AuthApiFactory factory) : IClassFixture<AuthApiFactory>
{
    [Fact]
    public async Task Endpoints_protegidos_rechazan_peticiones_anonimas()
    {
        var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var protegidos = factory.Services.GetRequiredService<EndpointDataSource>()
            .Endpoints.OfType<RouteEndpoint>()
            .Where(e => e.Metadata.GetOrderedMetadata<IAuthorizeData>().Any()
                        && e.Metadata.GetMetadata<IAllowAnonymous>() is null);

        var fallos = new List<string>();

        foreach (var endpoint in protegidos)
        {
            var method = endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()
                ?.HttpMethods.FirstOrDefault() ?? "GET";
            var url = Regex.Replace(endpoint.RoutePattern.RawText!,
                @"\{[^}]+\}", Guid.NewGuid().ToString());

            var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod(method), url));

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                fallos.Add($"{method} {endpoint.RoutePattern.RawText} -> {(int)response.StatusCode}");
        }

        Assert.True(fallos.Count == 0, string.Join(Environment.NewLine, fallos));
    }
}