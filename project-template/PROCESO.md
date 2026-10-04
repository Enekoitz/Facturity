# Proceso de trabajo

Playbook personal. Cambia poco, se reutiliza en todos los proyectos y se mejora con las retrospectivas.

| Fase | Pregunta | Documento | Salida |
|---|---|---|---|
| 1. Entender | ¿Por qué? | `vision.md`, `scope.md`, `stakeholders.md` | Visión, límites y mapa de personas |
| 2. Recopilar | ¿Qué sabemos? | `research/`, `glossary.md` | Notas en bruto y lenguaje común |
| 3. Requisitos | ¿Qué debe hacer? | `requirements.md` | Lista priorizada (MoSCoW) |
| 4. Historias | ¿Qué necesita el usuario? | `user-stories/` | Épicas con criterios de aceptación |
| 5. Dominio | ¿Sobre qué trabajamos? | `domain-model.md` | Entidades, relaciones y reglas |
| 6. Diseño | ¿Cómo? | `architecture.md`, `decisions/` | Arquitectura y ADRs |
| 7. Planificar | ¿Cuándo y quién? | Gestor de tareas (fuera del repo) | Tareas < 1 día, iteraciones, entregables |
| 8. Ejecutar | ¿Funciona? | Código + docs actualizadas | Entregas pequeñas y feedback |

## Reglas

- **Versión mala primero.** Una página por documento basta para avanzar.
- **Documentación = lo duradero.** Si se queda obsoleto en dos semanas, es gestión: va al gestor de tareas.
- **Una rama por unidad de cambio**, no por sesión de pensamiento. Si el cambio acompaña a código, la doc va en el mismo PR.
- **Los cambios de alcance o decisiones importantes** generan un ADR.
- **Las fases 3-5 se revisan en cada ciclo**, no se cierran de golpe.

## Retrospectiva (al terminar cada ciclo o proyecto)

- ¿Qué fase me costó más?
- ¿Qué documento no usé nunca?
- ¿Qué me faltó y me hizo perder tiempo?
- ¿Qué cambio en esta plantilla?
