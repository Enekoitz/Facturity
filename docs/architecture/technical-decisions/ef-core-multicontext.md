# EFCore_Multicontext

EF Core is implemented as an ORM to manage data access.

Each module has its own context associated with a specific database schema. This provides a level of encapsulation without requiring separate databases for each module—an approach that would introduce unnecessary complexity and resource consumption, given that we are not building microservices.

### `DbContext` filenames by module

| Module                                        | DbContext               | DB Schema   |
| --------------------------------------------- | ----------------------- | ----------- |
| UgeFactura.Modules.Clientes.Infrastructure    | ClientesDbContext.cs    | clientes    |
| UgeFactura.Modules.Facturacion.Infrastructure | FacturacionDbContext.cs | facturacion |
| ....                                          | ....                    | ....        |

### EF Core Global CLI Tool

To use EF Core via the CLI, you must have `dotnet-ef` installed:

```powershell
dotnet tool install --global dotnet-ef
```

### Migrations are performed from the Host

All migrations are executed from the host project, `UgeFactura.API`. Consequently, this project must have the `Microsoft.EntityFrameworkCore.Design` package installed and referenced. This package enables the execution of migrations—the only EF Core functionality required at this level of the system.

### History Tables

Each database schema maintains its own EF Core migration history table, named `__EFMigrationsHistory`.

Without this, all modules would share the same history table, which would lead to various issues.

This configuration is defined in the EF Core setup as follows:

```c#
// In the Clientes module
options.UseSqlServer(connectionString, x =>
x.MigrationsHistoryTable("__EFMigrationsHistory", "clientes"));
``` // <--- Dedicated table within its own schema

// In the Invoices (Facturas) module
options.UseSqlServer(connectionString, x =>
x.MigrationsHistoryTable("__EFMigrationsHistory", "facturas")); // <--- Dedicated table within its own separate schema

##### Advantages

- **Total isolation**: Migrations for a specific module/schema are stored in their own dedicated history table.

- **Cleanliness**: If you ever need to extract an entire module and assign it to an independent database (as you might when transitioning to microservices), you would simply need to move the corresponding schema.

- **No conflicts**: Database changes for one module can be applied without interfering with the operations of other modules within the system.


# Key EF Core Flags

- **`-p` (Project):** Where your classes and the `DbContext` are located (Infrastructure).

- **`-s` (Startup):** Where the configuration and the `ConnectionString` are located (API).

- **`-c` (Context):** Specifies which module you are currently working on.

### 1. Create the migration

```bash
dotnet ef migrations add Initial -p Path/To/Project.Infrastructure -s Path/To/Project.API --context ClientesDbContext
```

### 2. Apply the migration to the database

```bash
dotnet ef database update -p Path/To/Project.Infrastructure -s Path/To/Project.API --context ClientesDbContext
```

### 3. Automation at startup (optional)

A mechanism to automatically apply pending migrations when the application starts.

Add this to the `Program.cs` file:

```c#
using (var scope = app.Services.CreateScope())
{
var services = scope.ServiceProvider; 
try
{
var context = services.GetRequiredService<ClientesDbContext>(); 
// This applies pending migrations and creates the schema if it does not exist
if (context.Database.GetPendingMigrations().Any())
{
context.Database.Migrate(); 
}
}
catch (Exception ex)
{
var logger = services.GetRequiredService<ILogger<Program>>(); 
logger.LogError(ex, "An error occurred while applying migrations."); 
}
}
```



# EF Core Commands
## "Clients" Module

- Create the migration:

```bash
dotnet ef migrations add MigracionInicial -p .\Modules\Clientes\UgeFactura.Modules.Clientes.Infrastructure --output-dir Persistence\Migrations -s .\Host\UgeFactura.Api\ --context ClientesDbContext
```

- Apply the migration:

```c#
dotnet ef database update -p .\Modules\Clientes\UgeFactura.Modules.Clientes.Infrastructure\ -s .\Host\UgeFactura.API\ -c ClientesDbContext
```