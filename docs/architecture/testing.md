# Testing

Dada la importancia de mantener una arquitectura monolítica modular bien definida y con intenciones de extraer los módulos más demandantes a microservicios, se crea un proyecto de testing en .NET 10 por cada módulo desarrollado.

Permite la fácil migración de los módulos correspondientes, tanto el módulo operativo como el conjunto de pruebas adyacente, a microservicios.

Además, se utilizarán como parte del pipeline CI/CD en los despliegues, garantizando que, en caso de aplicar modificaciones en el código, el sistema pasará el filtro de pruebas automáticas minimizando el riesgo de fallas en producción.

## Características

- Framework: **.NET 10**
	
- Biblioteca empleadas: 
	- **xUnit**
	- **FakeTimeProvider**
	- **NetArchTest**
	- **FluentAssertions**

## Estructura

| Testing      | Nivel acoplamiento                                              |
| ------------ | --------------------------------------------------------------- |
| Unitario     | Módulo: `Facturity.<módulo>.UnitTests`                          |
| Integración  | Sistema completo (API + módulos): `Facturity.IntegrationTests`  |
| Arquitectura | Sistema completo (API + módulos): `Facturity.ArchitectureTests` |

## Convenciones

- Nomenclatura: _MethodName_StateUnderTest_ExpectedBehavior_

## Proyecto adicional de arquitectura

Se desarrolla también un  proyecto con tests de arquitectura. Es fundamental para que en cada build se verifiquen reglas y convenciones definidas por el equipo desarrollador que deben ser garantizadas sin excepción:

- **Referencias correctas** respetando la arquitectura definida, donde el Dominio es completamente independiente.
- **Módulos aislados** entre ellos, sin referencias cruzadas.
- **Módulos comunicados por contratos públicos** (eventos de integración, interfaces expuestas), nunca por tipos internos.

## Notas
Ninguna actualmente

---
_Última revisión: 2026-10-10
