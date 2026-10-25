---
status: accepted
date: 2026-02-14
---
# ADR 002: Adoption of a Hybrid Software Architecture (Clean Architecture + Vertical Slice)

## Context

During the application design phase—following the definition of system requirements and the deployment-level architecture (an evolutive modular monolith)—a need arises to define the software architecture (at the component or code-design level).

Which software architecture is best suited for the current context of the application's initial design phase?

## Decision

**A pragmatic hybrid architecture based on Clean Architecture and Vertical Slice.** We have chosen this approach because it aligns well with an evolutive modular monolithic architecture without locking the system into rigid structural patterns. It captures the benefits of each approach, adapted in a pragmatic—rather than dogmatic—manner.

## Alternatives Considered

- **Clean Architecture (pragmatic):** Each module functions as an assembly structured according to Clean Architecture principles: Domain, Application, and Infrastructure. The distinction lies in the application layer, which is organized by features.

- **Hexagonal Architecture (pragmatic):** Similar to the aforementioned Clean Architecture approach, but more flexible, without a strict separation between the domain and use cases (application layer).

- **Vertical Slice Architecture (VSA):** Instead of structuring modules horizontally—as in Clean or Hexagonal architectures—each feature encapsulates everything required for its operation (endpoint, command/query, handler, business logic, etc.).

## Consequences

- (+) Adheres to Dependency Rules (Domain <-- Application <-- Features).

- (+) Each module contains well-defined layers, safeguarded by architectural tests and disciplined practices.

- (+) Use cases are organized by features rather than large, generic services. Each feature contains all the components necessary to perform its specific operation, ensuring a semantic, readable, and maintainable structure. - (-) Because it is a hybrid and pragmatic architecture, there is a possibility of implementing or designing something that, in one way or another, compromises its underlying philosophy or dependency rules. In reality, this drawback applies to any architecture.

## Additional notes
(None)
## References
(None)