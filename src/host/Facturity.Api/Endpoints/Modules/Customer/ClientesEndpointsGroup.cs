namespace Facturity.Api.Endpoints;

public static class ClientesEndpointsGroup
{
    public static IEndpointRouteBuilder MapClientesEndpoints(this IEndpointRouteBuilder app)
    {
        var clientesGroup = app.MapGroup("/api/clientes")
                                .WithTags("Clientes");
        //TODO: .RequireAuthorization()

        var endpointTypes = typeof(ClientesEndpointsGroup).Assembly.GetTypes()
        .Where(t => typeof(IClientesEndpoint).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var type in endpointTypes)
        {
            var endpoint = (IClientesEndpoint)Activator.CreateInstance(type)!;
            endpoint.MapEndpoint(clientesGroup);
        }

        return app;
    }
}
