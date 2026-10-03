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
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseNpgsql(
            config.GetConnectionString(NombreConnectionString),
            o => o.MigrationsHistoryTable("__EFMigrationsHistory", "auth"));

        return new AuthDbContext(optionsBuilder.Options);
    }
}