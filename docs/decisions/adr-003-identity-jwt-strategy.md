---
status: accepted
date: 2026-10-03
---
# ADR 003: Identity Management, Authentication, and Authorization Strategy

## Context
Facturity functions as an API for internal use (consumed by an SPA or a proprietary client), requiring a stateless authentication system and identity (user) management.

Which strategy is the most suitable, robust, and sufficient for our specific context?

## Decision

**Adoption of ASP.NET Core Identity for identity management, and JWT for authentication and authorization via Bearer Tokens (custom implementation).**

**Why?**

- **Identity Management (ASP.NET Core Identity)**:

It provides a robust, battle-tested data model that integrates natively with Entity Framework Core and PostgreSQL via `IdentityDbContext`. 
The `IdentityUser` and `IdentityRole` abstractions allow for extending the model based on domain requirements without breaking compatibility with the .NET security ecosystem. 

- **Authentication and Authorization (JWT - Bearer Tokens)**:

This addresses the need for a stateless scheme for client-server communication. 
Implementing our own components for token generation, validation, and revocation (see the next ADR) gives us full control over claims, the token lifecycle, and revocation policies, without relying on third-party providers.

## Alternatives Considered

- Dedicated OIDC Server (Duende IdentityServer / Auth0 / Keycloak): This would be over-engineering for an internal-use system with no third-party consumers; it would only be worth considering if the system needed to interoperate with other clients or Identity Providers (IdPs).

- `MapIdentityApi` (out-of-the-box Identity endpoints): Uses opaque tokens and does not support custom claims (e.g., `tenant_id`) without requiring a rewrite of the login logic anyway (offering no real time savings).

## Consequences

- (+) Full control over claims and the token lifecycle.

- (+) Responsibility for token revocation lies with us (addressed via rotated refresh tokens and reuse detection—see the next ADR). - (=) If SSO/social login is required in the future, migrating to an OIDC server should be evaluated.

- (-) Increased development time, long-term maintainability considerations, and the need for integration tests to ensure the robustness and proper functioning of the implementation.

## Additional notes
(None at this time)

## References

- RFC Editor: [RFC 7519](https://www.rfc-editor.org/info/rfc7519/)