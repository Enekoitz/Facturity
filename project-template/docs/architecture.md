# Arquitectura

> **Propósito:** estructura técnica de alto nivel. Guía: modelo C4 (contexto → contenedores → componentes si hace falta).
>
> **Preguntas guía:** ¿Con qué sistemas se relaciona? ¿Qué piezas desplegables hay? ¿Qué tecnologías y por qué? ¿Qué decisiones están registradas?

## 1. Contexto
_El sistema, sus usuarios y los sistemas externos con los que interactúa._

```mermaid
flowchart LR
    U[Usuario] --> S[Sistema]
    S --> E[Sistema externo]
```

## 2. Contenedores / módulos principales
| Contenedor | Responsabilidad | Tecnología |
|---|---|---|
| | | |

## 3. Componentes (solo si aporta)
_Detalle interno de un contenedor complejo._

## Integraciones externas
| Sistema | Propósito | Protocolo / contrato |
|---|---|---|
| | | |

## Decisiones relevantes
_Enlaza los ADRs que explican esta arquitectura._

- [ADR-0001](decisions/0001-registrar-decisiones-como-adr.md)

---
_Última revisión: AAAA-MM-DD_
