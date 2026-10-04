---
status: accepted
date: 2026-10-03
---
# ADR 003: Estrategia de gestión de identidades, autenticación y autorización

## Contexto
Facturity trabaja como API de consumo propio (consumida por SPA o cliente propio) que requiere de un sistema de autenticación stateless y gestión de identidades (usuarios).

¿Qué estrategia es más adecuada, robusta y suficiente para nuestro contexto específico?

## Decisión

**Adopción de ASP.NET Core Identity como gestor de identidad y JWT para la autenticación y autorización mediante Bearer Tokens (implementación propia).**

**¿Por qué?**

  - **Gestión de Identidad (ASP.NET Core Identity)**:
    
    Proporciona un modelo de datos robusto y probado, integrable de forma nativa con Entity Framework Core y PostgreSQL mediante IdentityDbContext. 
    La abstracción de IdentityUser e IdentityRole permite extender el modelo según los requisitos del dominio sin romper la compatibilidad con el ecosistema de seguridad de .NET.
    
  - **Autenticación y Autorización (JWT - Bearer Tokens)**:

    Responde a la necesidad de un esquema stateless para la comunicación entre cliente y servidor. 
    La implementación propia de los componentes de generación, validación y revocación (siguiente ADR) nos otorga control total sobre los claims, el ciclo de vida del token y las políticas de revocación sin depender de proveedores terceros.

## Alternativas consideradas

- Servidor OIDC dedicado (Duende IdentityServer / Auth0 / Keycloak). Sobre-ingeniería para consumo propio sin terceros; valorable si el sistema necesitara interoperar con otros clientes/IdPs.
	
- MapIdentityApi (endpoints out-of-the-box de Identity). Token opaco, no permite claims custom (tenant_id) sin reescribir el login de todas formas (no ahorra trabajo real).

## Consecuencias

- (+) Control total sobre claims y ciclo de vida del token.
	
- (+) Responsabilidad propia de revocación (resuelto con refresh tokens rotados + detección de reuso, ver siguiente ADR).
	
- (=) Si en el futuro se necesita SSO/login social, hay que evaluar migrar a un servidor OIDC.
	
- (-) Incremento en el tiempo de desarrollo, mantenibilidad a futuro y necesidad de tests de integración que garanticen robustez y funcionamiento de la implementación.

### Notas adicionales
(Ninguna actualmente)

## Referencias

- RFC Editor: [RFC 7519](https://www.rfc-editor.org/info/rfc7519/)