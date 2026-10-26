using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Facturity.Modules.Auth.Infrastructure;

internal class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    private const string NombreConnectionString = "FacturityDbConnectionString";
    
    public AuthDbContext CreateDbContext(string[] args)
    {
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets("d53f5011-a08e-4a75-9221-6a2f0ac5dccc", reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseNpgsql(
            config.GetConnectionString(NombreConnectionString),
            o => o.MigrationsHistoryTable("__EFMigrationsHistory", "auth"));

        return new AuthDbContext(optionsBuilder.Options);
    }
}