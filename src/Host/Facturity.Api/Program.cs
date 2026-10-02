using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using Serilog;
using FluentValidation;
using Facturity.Api.Endpoints;
using Facturity.Api.Exceptions;
using Facturity.Api.Middlewares;
using Facturity.Api.Auth;
using Facturity.Modules.Bootstrapper;
using Facturity.Modules.Customer.Features;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{Application}/{MachineName}] [CorrelationId: {CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();

try
{
    const string nombreConnectionString = "FacturityDbConnectionString";
    const string nombreAppName = "Facturity";
    const string nombreAppNameApi = "FacturityAPI";
    
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
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddOpenApi($"{nombreAppNameApi}-v1");

    var connectionString = builder.Configuration.GetConnectionString(nombreConnectionString)
        ?? throw new InvalidOperationException($"CRITICAL: La cadena de conexión '{nombreConnectionString}' es nula.");

    builder.Services.AddValidatorsFromAssemblies(
        [
            typeof(CrearClienteCommand).Assembly
        ], 
        includeInternalTypes: true);
    
    builder.Services.AddAllModules(connectionString);
    builder.Services.AddJwtSettings(builder.Configuration);
    builder.Services.AddRefreshTokenSettings(builder.Configuration);
    
    var app = builder.Build();

    app.UseCorrelationId();
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle($"{nombreAppNameApi}")
                   .WithTheme(ScalarTheme.DeepSpace);
        });
        
        app.MapGet("/", () => Results.Redirect($"/scalar/{nombreAppNameApi}-v1"))
            .ExcludeFromDescription();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

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

// Used to be contemplated by integration testing WebApplicationFactory(Program) implementation when Program.cs is top-level
// statement. It lives in the global namespace so we have to do this.
public partial class Program { }