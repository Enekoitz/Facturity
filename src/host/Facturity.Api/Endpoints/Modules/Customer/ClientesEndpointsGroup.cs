namespace Facturity.Api.Endpoints.Modules.Customer;

internal static class ClientesEndpointsGroup
{
    internal static void MapClientesEndpoints(this IEndpointRouteBuilder app)
    {
        var clientesGroup = app
            .MapGroup("/api/clientes")
            .WithTags("Clientes");

        new ObtenerClientePorIdEndpoint().MapEndpoint(clientesGroup);
        new CrearClienteEndpoint().MapEndpoint(clientesGroup);
        new ListarClientesEndpoint().MapEndpoint(clientesGroup);
        new EliminarClienteEndpoint().MapEndpoint(clientesGroup);
        new DesactivarClienteEndpoint().MapEndpoint(clientesGroup);
    }
}
