# Arquitectura

> **Propósito:** estructura técnica de alto nivel. Guía: modelo C4 (contexto → contenedores → componentes si hace falta).
>
> **Preguntas guía:** ¿Con qué sistemas se relaciona? ¿Qué piezas desplegables hay? ¿Qué tecnologías y por qué? ¿Qué decisiones están registradas?

# Core

- [Arquitectura de sistema](architecture/system-architecture)
- [Arquitectura de software](architecture/software-architecture)
- [Patrones de diseño](architecture/design-patterns)
- [Testing](decisions/testing)

## 1. Contexto
==PDTE.==
_El sistema, sus usuarios y los sistemas externos con los que interactúa._

```mermaid
flowchart LR
    U[Usuario] --> S[Sistema]
    S --> E[Sistema externo]
```

## 2. Contenedores / módulos principales
==PDTE.==

| Contenedor | Responsabilidad | Tecnología |
| ---------- | --------------- | ---------- |
| Customer   |                 |            |
| Auth       |                 |            |
| Invoicing  |                 |            |


## 3. Componentes (solo si aporta)
_Detalle interno de un contenedor complejo._

## Integraciones externas
| Sistema | Propósito | Protocolo / contrato | Nota dedicada |
| ------- | --------- | -------------------- | ------------- |
|         |           |                      |               |

## Decisiones relevantes

- [ADR-01](adr-001-modular-monolith-as-system-architecture.md)
- [ADR-02](adr-002-clean-and-vertical-slice-as-software-hybrid-architecture.md)
---
_Última revisión: 2026-10-04_
