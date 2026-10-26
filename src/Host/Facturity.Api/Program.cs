using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Extensions.Hosting;
using FluentValidation;
using Facturity.Api.Endpoints;
using Facturity.Api.Exceptions;
using Facturity.Api.Middlewares;
using Facturity.Api.Auth;
using Facturity.Modules.Bootstrapper;
using Facturity.Modules.Customer.Features;

if (Log.Logger is not ReloadableLogger)
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] [{Application}/{MachineName}] [CorrelationId: {CorrelationId}] {Message:lj}{NewLine}{Exception}")
        .Enrich.FromLogContext()
        .CreateBootstrapLogger();
}

try
{
    const string nombreConnectionString = "FacturityDbConnectionString";
    const string nombreAppName = "Facturity";
    const string nombreAppNameApi = "FacturityAPI";
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services),
        preserveStaticLogger: true);
    
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
        });
    
    builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommand>(includeInternalTypes: true);
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddOpenApi($"{nombreAppNameApi}-v1");

    var connectionString = builder.Configuration.GetConnectionString(nombreConnectionString)
        ?? throw new InvalidOperationException($"CRITICAL: The connection string '{nombreConnectionString}' is nula.");

    builder.Services.AddValidatorsFromAssemblies(
        [
            typeof(CreateCustomerCommand).Assembly
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
    Log.Fatal(ex, "The application failed during startup.");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }