Se implementa EF Core como ORM para gestionar el acceso a datos.

Cada módulo tiene su propio contexto asociado a un esquema en base de datos, lo que garantiza o proporciona cierto nivel de encapsulamiento sin llegar a separar en base de datos individuales por módulo, lo que proporcionaría exceso de complejidad y consumo de recursos.

### Nombres de los archivos `DbContext` según módulo

| Módulo                                        | DbContext               | Esquema BD  |
| --------------------------------------------- | ----------------------- | ----------- |
| UgeFactura.Modules.Clientes.Infrastructure    | ClientesDbContext.cs    | clientes    |
| UgeFactura.Modules.Facturacion.Infrastructure | FacturacionDbContext.cs | facturacion |
| ....                                          | ....                    | ....        |

### Herramienta global EF Core para CLI

Para utilizar EF Core por CLI hace falta tener lo siguiente instalado `dotnet-ef`:

```poweshell
dotnet tool install --global dotnet-ef
```

### Las migraciones se realizan desde el Host

Todas las migraciones se realizan desde el proyecto Host `UgeFactura.API`. Por lo que el proyecto tiene que tener instalado y referenciado el paquete `Microsoft.EntityFrameworkCore.Design`. 
Dicho paquete permite ejecutar migraciones, lo único que necesitamos de EF Core en ese punto del sistema.

### Tablas de historial

Cada esquema de BD también tiene su propia tabla histórica de migraciones EF Core `__EFMigrationsHistory`. 

De lo contrario, todos los módulos compartirían la misma tabla histórica, lo generaría problemas varios.

Con la siguiente instrucción de código en el archivo de configuración de EF Core se define dicha condición:

```c#
// En el módulo de Clientes
options.UseSqlServer(connectionString, x => 
    x.MigrationsHistoryTable("__EFMigrationsHistory", "clientes")); // <--- Tabla propia en su esquema

// En el módulo de Facturacion
options.UseSqlServer(connectionString, x => 
    x.MigrationsHistoryTable("__EFMigrationsHistory", "facturacion"));  // <--- Tabla propia en su otro esquema
```

##### Ventajas

- **Aislamiento total**: Las migraciones de un módulo/esquema se guardan en su tabla histórica específica.

- **Limpieza**: En caso de querer abstraer un módulo completo y asignarle una BD independiente (como haríamos si pasásemos a microservicios), solo se tendría que pasar el esquema completo correspondientes. 

- **Sin conflictos**: Se aplican los cambios de BD de un módulo sin interferir en la aplicación que realice otro módulo del sistema.