# ADR-0001: Registrar decisiones como ADR

- **Estado:** Aceptado
- **Fecha:** AAAA-MM-DD

## Contexto
Las decisiones importantes se olvidan o se cuestionan sin saber por qué se tomaron.

## Decisión
Registrar cada decisión relevante como un ADR numerado en `docs/decisions/`, inmutable una vez aceptado.

## Alternativas consideradas
| Alternativa | Pros | Contras |
|---|---|---|
| Wiki externa | Fácil de editar | Se desincroniza del código |
| No registrar | Cero esfuerzo | Se pierde el contexto |

## Consecuencias
- **Positivas:** historial claro del porqué del proyecto.
- **Negativas:** pequeño coste de redacción por decisión.
