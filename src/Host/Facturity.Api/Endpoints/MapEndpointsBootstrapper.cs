using Facturity.Api.Endpoints.Modules.Auth;
using Facturity.Api.Endpoints.Modules.Customer;

namespace Facturity.Api.Endpoints;

internal static class MapEndpointsBootstrapper
{
    internal static void UseApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapClientesEndpoints();
        app.MapAuthEndpoints();
    }
}