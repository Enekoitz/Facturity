# EF Core Migrations Managemente at Facturity
## Migration creation in Facturity
### 1. Set up

Open the terminal and position yourself at `/src`:

```cmd
{AbsolutRoute}/Facturity/src
```
### 2. Create the migration 
Each module has its context:
#### Customer

```bash
dotnet ef migrations add "Initial" -p .\Modules\Customer\Facturity.Modules.Customer\ -s .\Host\Facturity.Api\ --context CustomerDbContext
```
#### Invoicing

```bash
dotnet ef migrations add "Initial" -p .\Modules\Invoicing\Facturity.Modules.Invoicing\ -s .\Host\Facturity.Api\ --context InvoicingDbContext
```


>_Note: As modules are added, update here_

### 3. Apply the migration to the database

```bash
dotnet ef database update -p .\Modules\Invoicing\Facturity.Modules.Invoicing\ -s .\Host\Facturity.Api\ --context InvoicingDbContext
```

---
### In production 

An idempotent SQL script (`dotnet ef migrations script --idempotent`) or a migration bundle (`dotnet ef migrations bundle`) is generated, which is then executed as a step in the deployment process. 
Migrations are not applied during the application startup.