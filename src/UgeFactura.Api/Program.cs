using Microsoft.EntityFrameworkCore;
using UgeFactura.Application.Clientes;
using UgeFactura.Infrastructure.Persistencia;
using UgeFactura.Infrastructure.Persistencia.Repositorios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UgeFacturaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UgeFactura")
    )
);

//builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
