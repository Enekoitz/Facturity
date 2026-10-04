# Decisiones de arquitectura (ADRs)

> **Propósito:** registrar el *porqué* de las decisiones importantes. Un ADR por decisión, numerado.
>
> **Regla:** los ADRs son **inmutables**. Si una decisión cambia, se crea un ADR nuevo que sustituye al anterior (y el viejo pasa a "Sustituido").

## Cuándo crear uno

- Cambios de alcance.
	
- Elección de tecnología, base de datos, patrón o proveedor.
	
- Cualquier decisión difícil de revertir o que alguien cuestionará en seis meses.

## Cómo

1. Copia [`adr-000-template-adr.md`](adr-000-template-adr.md).
2. Numera y nombra: `adr-014-implementacion-algoritmo-firma-factura.md`.
3. Añádelo al índice de abajo.

## Índice

| Nº                                                                                | Título                                                                             | Estado   | Fecha      |
| --------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------- | -------- | ---------- |
| [001](decisions/adr-001-modular-monolith-as-system-architecture)                  | Adopción de Arquitectura Monolítica Modular                                        | Aceptado | 2026-01-26 |
| [002](decisions/adr-002-clean-and-vertical-slice-as-software-hybrid-architecture) | Adopción de Arquitectura de Software Híbrida (Clean Architecture + Vertical Slice) | Aceptado | 2026-02-14 |
| [003](decisions/adr-003-identity-jwt-strategy)                                    | Estrategia de gestión de identidades, autenticación y autorización                 | Aceptado | 2026-10-03 |
| [004](decisions/adr-004-refresh-token-rotation)                                   | Estrategia de rotación de refresh token                                            | Aceptado | 2026-10-03 |
