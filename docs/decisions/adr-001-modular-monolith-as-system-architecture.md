---
status: accepted
date: 2026-01-26
---
# ADR 001: Adoption of a Modular Monolithic Architecture

## Context

During the application design phase, following the definition of system requirements, we must decide on the system or deployment architecture to be implemented.

As this is an invoicing application with additional multi-tenant business management modules, the volume of traffic and concurrent usage is not expected to be very high; therefore, the initial load should remain manageable.

It is crucial to note that we cannot accurately estimate the actual load, traffic, or specific challenges we might face until the first version is released to production and has been in use for some time.

Which architecture is best suited for the current initial design phase of the application?

## Decision

**Modular monolithic architecture**. We have chosen this approach because it minimizes initial operational complexity and infrastructure costs, while simultaneously ensuring a clear migration path toward microservices.

## Alternatives Considered

- **Microservices**. The application would consist of independent microservices that communicate via various infrastructure and network mechanisms—a highly distributed system.

## Consequences

- (+) Reduces the complexity associated with microservices, such as infrastructure management, maintenance, and network latency.

- (+) Simplifies and streamlines refactoring.

- (+) Simplifies deployment, as only a single compiled artifact needs to be deployed.

- (+) Promotes disciplined isolation and encapsulation of modules, preventing circular dependencies, tight coupling, and the emergence of a "Big Ball of Mud."

- (+) Ensures more robust and consistent database transactions across modules.

- (+) Facilitates the extraction of specific modules into microservices—should the need for scaling (or other reasons) arise—while allowing the monolith to coexist with individual microservices. - (-) If necessary, the only option is to scale the entire monolith, even if only one specific module is bearing the brunt of the load. (This could be resolved by extracting that module into a microservice.)

- (-) Ideally, each module should own its own data (separate tables, separate schemas, or at least a strict logical separation at the repository level).

- (-) It is easy for developers to bypass the boundaries, conventions, and rules established to maintain isolation between modules. (Potential solutions include defining and implementing specific tools and mechanisms—alongside strong team discipline and communication—such as NetArchTest, folder structure validation within the CI/CD pipeline, etc.)

- (-) All modules are coupled to the same technology stack.

- (-) If a module contains a critical bug that crashes the application, the entire platform goes down, affecting other modules that were otherwise functioning perfectly.

## Additional Notes
- Having opted for a modular monolithic architecture, the next step is to determine which specific software architecture (e.g., Clean, Hexagonal, Vertical Slice, or a hybrid approach) will be implemented.

## References
(None)