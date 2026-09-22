---
status: accepted
date: 2026-02-14
---
# ADR 02: Adopción de Arquitectura de Software Híbrida (Clean Architecture + Vertical Slice)
## Contexto
Durante la fase de diseño de la aplicación, tras haber definido los requerimientos del sistema y la arquitectura a nivel de despliegue (Monolito modular evolutivo) surge la necesidad de definir la arquitectura de software (a nivel de componente o diseño de código).

¿Qué arquitectura de software es la más adecuada para el contexto actual de diseño inicial de la aplicación?
## Decisión
**Arquitectura híbrida pragmática basada en Clean Architecture y Vertical Slice** . Elegimos está opción porque calza muy bien con la arquitectura monolítica modular evolutiva sin comprometer el sistema a arquitecturas rígidas. Obteniendo los beneficios de cada arquitectura adaptándolo a una forma pragmática y no dogmática.
## Alternativas consideradas
- **Clean Architecture (pragmático)**. Cada módulo sería un ensamblado con una estructura al estilo CA: Dominio, Aplicación e Infraestructura. La diferencia estaría en la capa de aplicación en vez de organizar por features.
- **Hexagonal Architecture (pragmático)**. Igual que la alternativa anterior, Clean Architecture, solo que más flexible y sin diferenciar el dominio de los casos de uso (aplicación).
- **Vertical Slice Architecture (VSA)**. En vez de estructurar los módulos de forma horizontal al estilo clean o hexagonal, cada feature aglutina todo lo necesario para poder operar (endpoint, command/query, handler, lógica de negocio, etc.)
## Consecuencias
- (+) Se respeta las Dependency Rules (Domain <-- Application <-- Features)
- (+) Cada módulo contiene sus capas bien definidas y protegidas por tests de arquitectura y disciplina.
- (+) Los casos de uso se organizan por features en vez de servicios genéricos y grandes. Cada feature contiene sus componentes necesarios para realizar la operación. Mantiene un orden semántico, legible y mantenible.
- (-) Al ser una arquitectura híbrida y pragmática existe la posibilidad de implementar o diseñar algo que corrompa de una forma u otra la filosofía o las dependency rules. En realidad, esta contra aplica a cualquier arquitectura.
### Notas adicionales
(Ninguna)
## Referencias
(Ninguna)