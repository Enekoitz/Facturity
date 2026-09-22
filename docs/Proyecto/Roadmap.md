# 🗺️ Roadmap Proyecto Portfolio .NET Backend

---

## FASE DE PLANIFICACIÓN

### Fase 1 — Definir el proyecto ⭐ Clave
**Tiempo estimado: 2–3 h**

- Escribe en 1 párrafo qué hace el sistema y a quién ayuda
- Lista 5–8 funcionalidades core (no más). Descarta el resto
- Define los actores: quién usa el sistema y qué necesita
- Crea un README.md inicial: descripción, tech stack, objetivos

---

## Fase 2 — Diseño de arquitectura
**Tiempo estimado: 3–4 h**

- Elige la arquitectura: Clean Arch, Vertical Slice, Modular Monolith
- Define las entidades y agregados del dominio (clases C#, no ERD)
- Diseña los endpoints REST o contratos de la API en papel
- Dibuja el flujo de una petición de punta a punta
- El ERD se genera como subproducto de las migraciones de EF Core

---

## FASE DE CONSTRUCCIÓN

### Fase 3 — Setup del proyecto ✅ Una vez
**Tiempo estimado: 2–3 h**

- Repo en GitHub con estructura de carpetas definitiva
- Solution con proyectos: API, Application, Domain, Infrastructure
- Configura: EF Core, Serilog, FluentValidation, MediatR si usas CQRS
- Proyecto de tests: xUnit + estructura de carpetas de tests
- docker-compose con la base de datos

---

### Fase 4 — Desarrollo por features 🔁 Iterar
**Tiempo estimado: 1–3 días / feature**

- Una rama por feature (feat/auth, feat/products…)
- Orden dentro de cada feature: Domain → Application → Infrastructure → API
- Tests unitarios al mismo tiempo que el código, no después
- PR a main con descripción de qué resuelve cada cambio

---

## FASE DE CALIDAD Y ENTREGA

### Fase 5 — Tests e integración
**Tiempo estimado: 1–2 días**

- Tests de integración con base de datos real (Testcontainers)
- Tests de los flujos críticos end-to-end
- Cobertura mínima: 70–80% en capa de Application

---

### Fase 6 — Deploy y documentación
**Tiempo estimado: 1 día**

- GitHub Actions: build + tests en cada push
- Deploy a Railway, Render o Azure App Service
- Swagger/OpenAPI disponible en producción
- README final: cómo correrlo, decisiones de arquitectura, aprendizajes

---

## Resumen

| | |
|---|---|
| **Tiempo total estimado** | 2–4 semanas |
| **Dedicación recomendada** | 2–3 h / día |
| **Features máximo** | 5–6 |

> 💡 **Regla de oro:** entrega un proyecto pequeño pero impecable antes que uno grande e inacabado.
> Los recruiters valoran el README, los tests y el historial de commits.