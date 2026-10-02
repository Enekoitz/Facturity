namespace Facturity.Api.Endpoints.Modules.Customer;

internal static class ClientesEndpointsGroup
{
    internal static void MapClientesEndpoints(this IEndpointRouteBuilder app)
    {
        var clientesGroup = app
            .MapGroup("/customer")
            .WithTags("Clientes");

        new CrearClienteEndpoint().MapEndpoint(clientesGroup);
        new ActualizarClienteEndpoint().MapEndpoint(clientesGroup);
        new ObtenerClientePorIdEndpoint().MapEndpoint(clientesGroup);
        new ListarClientesEndpoint().MapEndpoint(clientesGroup);
        new DesactivarClienteEndpoint().MapEndpoint(clientesGroup);
        new ActivarClienteEndpoint().MapEndpoint(clientesGroup);
        new EliminarClienteEndpoint().MapEndpoint(clientesGroup);
    }
}
