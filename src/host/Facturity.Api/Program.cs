using Facturity.Api.Endpoints;
using Facturity.Api.Exceptions;
using Facturity.Api.Middlewares;
using Facturity.Modules.Bootstrapper;
using Facturity.Modules.Customer.Features;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;
using Facturity.Api;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

//Serilog.Debugging.SelfLog.Enable(Console.Error);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{Application}/{MachineName}] [CorrelationId: {CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341", apiKey: "[REDACTED]")
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
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
    
    builder.Services.AddValidatorsFromAssemblyContaining<CrearClienteCommand>();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddOpenApi("FacturityAPI-v1");

    var connectionString = builder.Configuration.GetConnectionString("FacturityDbConnectionString")
        ?? throw new InvalidOperationException("CRITICAL: La cadena de conexión 'FacturityDbConnectionString' es nula.");

    builder.Services.AddValidatorsFromAssemblies(
        [
            typeof(CrearClienteCommand).Assembly
        ], 
        includeInternalTypes: true);
    
    builder.Services.AddCustomerModuleServices(connectionString);
    
    var app = builder.Build();

    app.UseCorrelationId();
    app.UseExceptionHandler();

    Log.Information("Middlewares personalizados para el tracking de peticiones HTTP con CorrelationId y manejador de excepciones global implementado en la aplicación.");
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("FacturityAPI")
                   .WithTheme(ScalarTheme.DeepSpace);
        });
        
        app.MapGet("/", () => Results.Redirect("/scalar/FacturityAPI-v1"))
            .ExcludeFromDescription();
    }

    app.UseHttpsRedirection();

    //app.UseAuthentication();
    //app.UseAuthorization();

    app.UseApplicationEndpoints();
    
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "La aplicación falló durante el arranque.");
}
finally
{
    Log.CloseAndFlush();
}