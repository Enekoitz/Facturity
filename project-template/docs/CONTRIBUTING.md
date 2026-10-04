# Cómo contribuir

## Flujo de trabajo (GitHub flow)

1. Crea una rama desde `main` con prefijo según el tipo de cambio:
   - `feature/...`, `fix/...`, `docs/...`
2. Haz commits pequeños y con mensaje claro.
3. Abre un PR usando la plantilla.
4. Fusiona tras revisión (o autorrevisión si trabajas solo).

## Reglas para la documentación

- **Una rama por unidad de cambio**, no por sesión de pensamiento.
- Si el cambio de código afecta a la doc, **actualízala en el mismo PR**.
- Cambios de alcance, arquitectura o decisiones importantes → **nuevo ADR**.
- Correcciones menores (erratas, glosario) pueden ir directas a `main` si trabajas solo.
- Backlog y tareas **no** se documentan aquí: van al gestor de tareas.

## Convenciones
- Commits: `tipo: descripción` (`docs: actualizar alcance`, `feat: ...`).
- Nombres de archivo en minúsculas y con guiones.
- Diagramas con Mermaid siempre que sea posible.
- Cada documento termina con `Última revisión: fecha`.
