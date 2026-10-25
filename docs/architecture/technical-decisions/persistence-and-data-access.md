# Data Persistence and Access

## Engine
PostgreSQL 16. Chosen for being open source, robust for relational data, and having strong support within the .NET ecosystem via Npgsql.

## ORM
Entity Framework Core with the Npgsql provider.

## Conventions

- `snake_case` naming is explicitly defined within each `IEntityConfiguration`.

- The `EFCore.NamingConventions` package is not used, in order to maintain full control over each column → [ADR-00X Explicit Naming](../../decisiones/ADR-00X...)

## Migrations
Managed using EF Core Migrations. These are applied manually during development and as part of the pipeline in production.

## Key Decisions

- A multi-context approach is used for EF Core, with one context per module, each mapped to its own schema.
- The Repository pattern is employed to decouple the infrastructure layer from the application layer (this does not apply to modules such as authentication, where it would constitute over-engineering).