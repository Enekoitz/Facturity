
### Módulo "Clientes"

- Crear la migración:

```bash
dotnet ef migrations add MigracionInicial -p .\Modules\Clientes\UgeFactura.Modules.Clientes.Infrastructure --output-dir Persistence\Migrations -s .\Host\UgeFactura.Api\ --context ClientesDbContext
```

- Aplicar la migración:

```c#
dotnet ef database update -p .\Modules\Clientes\UgeFactura.Modules.Clientes.Infrastructure\ -s .\Host\UgeFactura.API\ -c ClientesDbContext
```