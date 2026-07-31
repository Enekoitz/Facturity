namespace UgeFactura.Api;

public static class ClientesEndpointsGroup
{
    public static IEndpointRouteBuilder MapClientesEndpoints(this IEndpointRouteBuilder app)
    {
        var clientesGroup = app.MapGroup("/api/clientes")
                                .WithTags("Clientes");
        //TODO: .RequireAuthorization()

        var endpointTypes = typeof(ClientesEndpointsGroup).Assembly.GetTypes()
        .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(type)!;
            endpoint.MapEndpoint(clientesGroup);
        }

        return app;
    }
}
