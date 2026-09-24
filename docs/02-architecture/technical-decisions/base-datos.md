# Base de datos

## Motor
PostgreSQL 16. Elegido por ser open source, robusto para datos relacionales
y con buen soporte en el ecosistema .NET con Npgsql.

## ORM
Entity Framework Core con el provider de Npgsql.

## Convenciones
	
- Nomenclatura snake_case definida explícitamente en cada IEntityConfiguration
	
- No se usa el paquete EFCore.NamingConventions para mantener control total
  sobre cada columna → [ADR-00X Naming explícito](../../decisiones/ADR-00X...)

## Migraciones
Gestionadas con EF Core Migrations. Se aplican manualmente en desarrollo
y como parte del pipeline en producción.

## Decisiones relevantes