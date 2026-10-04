---
status: accepted
date: 2026-01-26
---
# ADR 001: Adopción de Arquitectura Monolítica Modular

## Contexto

Durante la fase de diseño de la aplicación, tras haber definido los requerimientos del sistema, nos surge la necesidad de decidir la arquitectura de sistema o despliegue que se va a emplear. 

Al ser una aplicación de facturación con módulos adicionales de gestión empresarial multi-tenant, la cantidad de tráfico y consumo concurrente no es demasiado grande, por lo que la carga en un principio no debería de ser muy alta. 

Es fundamental tener en cuenta que hasta que no salga a producción una primera versión y no lleve un tiempo en uso, no podremos estimar con exactitud la carga, tráfico y problemáticas concretas que tendremos.

¿Qué arquitectura es la más adecuada para el contexto actual de diseño inicial de la aplicación?

## Decisión

**Arquitectura monolítica modular**. Elegimos esta opción porque minimiza la complejidad operativa inicial y el costo de infraestructura, garantizando al mismo tiempo un camino de migración limpio hacia microservicios.

## Alternativas consideradas

- **Microservicios**. La aplicación estaría compuesta por microservicios independientes comunicados entre sí por distintos mecanismos de infraestructura y red. Un sistema altamente distribuido.

## Consecuencias

- (+) Reduce la complejidad de infraestructura, mantenimiento, latencia de red, etc. de los microservicios.
	
- (+) Refactorización más simple y ágil.
	
- (+) El despliegue es sencillo porque solo se despliega un único compilado.
	
- (+) Aislamiento y encapsulación disciplinada de los módulos; evitando referencias circulares y acoplamiento fuerte entre módulos y Big Ball of Mud.
	
- (+) Transacciones de BD más sólidas y consistentes entre módulos.
	
- (+) Posibilita y facilita enormemente la extracción de módulos con necesidad de escalado (o por otros motivos) a microservicios. Permitiendo la convivencia del monolito con microservicios concretos.
	
- (-) De ser necesario, solo es posible escalar el monolito al completo, aunque sea un módulo concreto el que está recibiendo la mayor parte de dicha carga. (Se podría solucionar extrayendo el módulo a microservicio).
	
- (-) Cada módulo debería idealmente poseer sus propios datos (tablas separadas, esquemas separados o al menos una separación lógica estricta a nivel de repositorio).
	
- (-) Es fácil que los desarrolladores se salten los límites, convenciones, reglas definidas para mantener ese aislamiento entre módulos. (Como solución habría que definir e implementar herramientas y mecanismos concretos, a parte de una buena disciplina y comunicación de equipo: NetArchTest, validaciones de estructura de carpetas en el pipeline de CI/CD, etc.).
	
- (-) Todos los módulos quedan acoplados a la misma tecnología.
	
- (-) Si un módulo tiene un bug crítico que tira la aplicación, se cae toda la plataforma, afectando a los demás módulos que funcionaban perfectamente.

### Notas adicionales
- Bajo el contexto de haber decidido la arquitectura monolítica modular hay que decidir también en los próximos pasos qué tipo de arquitectura de software (Clean, Hexagonal, Vertical Slice, híbrido, etc.) se va a implementar.

## Referencias
(Ninguna)