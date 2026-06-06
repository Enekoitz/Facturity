
- **`-p` (Project):** Dónde están tus clases y el `DbContext` (Infrastructure).
    
- **`-s` (Startup):** Dónde está la configuración y el `ConnectionSring` (API).
    
- **`-c` (Context):** Cuál de los módulos estás tocando en ese momento.

### 1. Crear la migración

```bash
dotnet ef migrations add Inicial -p Ruta/Al/Proyecto.Infrastructure -s Ruta/Al/Proyecto.API --context ClientesDbContext
```

### 2. Aplicar la migración a la base de datos

```bash
dotnet ef database update -p Ruta/Al/Proyecto.Infrastructure -s Ruta/Al/Proyecto.API --context ClientesDbContext
```

### 3. Automatización en el inicio (opcional)

Mecanismo para aplicar migraciones pendientes automáticamente al iniciar la ejecución de la aplicación. 

Añadir en el archivo `Program.cs`:

```c#
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ClientesDbContext>();
        // Esto aplica las migraciones pendientes y crea el esquema si no existe
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al aplicar las migraciones.");
    }
}
```

