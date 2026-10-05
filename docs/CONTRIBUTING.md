# Cómo contribuir

## Flujo de trabajo (GitHub flow)

1. Crea una rama desde `main` con prefijo según el tipo de cambio:
   - `feature/...`, `fix/...`, `docs/...`
	
1. Haz commits pequeños y con mensaje claro.
	
2. Abre un PR usando la plantilla.
	
3. Fusiona tras revisión (o autorrevisión si trabajas solo).

## Reglas para la documentación

- **Una rama por unidad de cambio**, no por sesión de pensamiento.
	
- Si el cambio de código afecta a la doc, **actualízala en el mismo PR** (`docs-as-code`).
	
- Cambios de alcance, arquitectura o decisiones importantes → **nuevo ADR**.
	
- Correcciones menores (erratas, glosario) pueden ir directas a `main` si trabajas solo.
	
- Backlog y tareas **no** se documentan aquí: van al gestor de tareas **GitHub Issues**.

## Convenciones

- Commits: `tipo: descripción` (`docs: actualizar alcance`, `feat(api): añadir manejador global de excepciones`, etc.).
	
- Nombres de archivo en minúsculas y con guiones.
	
- Diagramas con Mermaid siempre que sea posible.
	
- Cada documento termina con `Última revisión: fecha`.
