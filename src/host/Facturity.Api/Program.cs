using Facturity.Api;
using Facturity.Modules.Bootstrapper;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;

//Serilog.Debugging.SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341", apiKey: "[REDACTED]")
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando el host de Facturity...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
        });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddOpenApi("v1");

    var connectionString = builder.Configuration.GetConnectionString("FacturityDbConnectionString")
        ?? throw new InvalidOperationException("CRITICAL: La cadena de conexión 'FacturityDbConnectionString' es nula.");

    builder.Services.AddCustomerModuleServices(connectionString);

    var app = builder.Build();

    app.UseCorrelationId();
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options.WithTitle("API Facturity")
                   .WithTheme(ScalarTheme.DeepSpace);
        });
    }

    app.UseHttpsRedirection();

    //app.UseAuthentication();
    //app.UseAuthorization();

    app.MapClientesEndpoints();
    // TODO: A medida que avancemos con los endpoints añadimos:
    //app.MapProductosEndpoints();
    //app.MapFacturacionEndpoints();
    //app.MapMotorImpuestosEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló durante el arranque.");
}
finally
{
    Log.CloseAndFlush();
}