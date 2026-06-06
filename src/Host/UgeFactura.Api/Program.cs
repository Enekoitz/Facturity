using UgeFactura.Api;
using UgeFactura.Modules.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS8604 // Posible argumento de referencia nulo
var connString = builder.Configuration.GetConnectionString("UgeFacturaDbConnection");
builder.Services.AddAllModules(connString);
#pragma warning restore CS8604 // Posible argumento de referencia nulo

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        // Forzamos la ruta exacta donde .NET genera el JSON
        options.SwaggerEndpoint("/openapi/v1.json", "API UgeFactura");
        // Opcional: Esto hace que Swagger sea la página de inicio (http://localhost:XXXX/)
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// TODO: Implementar la autenticación y autorización:
//app.UseAuthentication();
//app.UseAuthorization();

app.MapClientesEndpoints();

// TODO: A medida que avancemos con los endpoints añadimos:
//app.MapProductosEndpoints();
//app.MapFacturacionEndpoints();
//app.MapMotorImpuestosEndpoints();

app.Run();