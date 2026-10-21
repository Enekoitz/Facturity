using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Facturity.Modules.Invoicing.Infrastructure;

public class InvoicingDbContextFactory : IDesignTimeDbContextFactory<InvoicingDbContext>
{
    private const string NombreConnectionString = "FacturityDbConnectionString";
    
    public InvoicingDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<InvoicingDbContext>()
            .UseNpgsql(config.GetConnectionString(NombreConnectionString),
                        o => o.MigrationsHistoryTable("__EFMigrationsHistory", "invoicing"));

        return new InvoicingDbContext(optionsBuilder.Options);
    }
}