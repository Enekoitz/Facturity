Dada la importancia de mantener una arquitectura monolítica modular bien definida y con intenciones de extraer los módulos más demandantes a microservicios, se crea un proyecto de testing en .NET 10 por cada módulo desarrollado.

Permite la fácil migración de los módulos correspondientes, tanto el módulo operativo como el conjunto de pruebas adyacente, a microservicios.

Además, se utilizarán como parte del pipeline CI/CD en los despliegues, garantizando que, en caso de aplicar modificaciones en el código, el sistema pasará el filtro de pruebas automáticas minimizando el riesgo de fallas en producción.
### Características
Cada proyecto modular tiene las siguientes características:
- Framework: **.NET 10**
- Biblioteca de tests unitarios: **xUnit**
### Estructura
Un proyecto de testing debe contener la siguiente estructura acorde con el sistema UgeFactura:

| Capa           | Testing     | Referencias                                                     | Características                          |
| -------------- | ----------- | --------------------------------------------------------------- | ---------------------------------------- |
| Domain         | Unitario    | `<módulo>.Domain`                                               | Sin dependencias externas, solo xUnit    |
| Features       | Unitario    | `<módulo>.Domain` `<módulo>.Features`                           | Mocks de puertos/repos (Moq/NSubstitute) |
| Infrastructure | Integración | `<módulo>.Domain` `<módulo>.Features` `<módulo>.Infrastructure` | DB real/Testcontainers, mensajería, etc. |
### Proyecto adicional de arquitectura
Se desarrolla también un  proyecto con tests de arquitectura. Es fundamental para que en cada build se verifiquen reglas y convenciones definidas por el equipo desarrollador que deben ser garantizadas sin excepción:

- **Referencias correctas** respetando la arquitectura definida, donde el Dominio es completamente independiente.
- **Módulos aislados** entre ellos, sin referencias cruzadas.
- **Módulos comunicados por contratos públicos** (eventos de integración, interfaces expuestas), nunca por tipos internos.

Herramienta a utilizar: **NetArchTest**


