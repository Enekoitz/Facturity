using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Facturity.Modules.Customer.Infrastructure;

public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
{
    private const string NombreConnectionString = "FacturityDbConnectionString";
    
    public CustomerDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
        optionsBuilder.UseNpgsql(
            config.GetConnectionString(NombreConnectionString),
            o => o.MigrationsHistoryTable("__EFMigrationsHistory", "customer"));

        return new CustomerDbContext(optionsBuilder.Options);
    }
}