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
    public async Task Protected_endpoints_reject_anonymous_requests()
    {
        var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var protectedEndpoints = factory.Services.GetRequiredService<EndpointDataSource>()
            .Endpoints.OfType<RouteEndpoint>()
            .Where(e => e.Metadata.GetOrderedMetadata<IAuthorizeData>().Any()
                        && e.Metadata.GetMetadata<IAllowAnonymous>() is null);

        var errors = new List<string>();

        foreach (var endpoint in protectedEndpoints)
        {
            var method = endpoint.Metadata.GetMetadata<IHttpMethodMetadata>()
                ?.HttpMethods.FirstOrDefault() ?? "GET";
            var url = Regex.Replace(endpoint.RoutePattern.RawText!,
                @"\{[^}]+\}", Guid.NewGuid().ToString());

            var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod(method), url));

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                errors.Add($"{method} {endpoint.RoutePattern.RawText} -> {(int)response.StatusCode}");
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }
}