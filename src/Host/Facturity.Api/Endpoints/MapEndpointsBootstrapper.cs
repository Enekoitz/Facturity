using Facturity.Api.Endpoints.Modules.Customer;

namespace Facturity.Api.Endpoints;

internal static class MapEndpointsBootstrapper
{
    internal static IEndpointRouteBuilder  UseApplicationEndpoints(this IEndpointRouteBuilder  app)
    {
        app.MapClientesEndpoints();
        return app;
    }
}