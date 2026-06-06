namespace UgeFactura.Api
{
    public static class ClientesEndpointsGroup
    {
        public static IEndpointRouteBuilder MapClientesEndpoints(this IEndpointRouteBuilder app)
        {
            var productsGroup = app.MapGroup("")
                                    .WithTags("Clientes");
                             //TODO: .RequireAuthorization()

            productsGroup.MapCrearCliente();
              
            return app;
        }
    }
}
