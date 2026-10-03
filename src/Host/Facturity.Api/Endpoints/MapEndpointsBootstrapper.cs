using Facturity.Api.Endpoints.Modules.Auth;
using Facturity.Api.Endpoints.Modules.Customer;

namespace Facturity.Api.Endpoints;

internal static class MapEndpointsBootstrapper
{
    internal static void UseApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app.MapGroup("/api");
        
        apiGroup.MapClientesEndpoints();
        apiGroup.MapAuthEndpoints();
    }
}