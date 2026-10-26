# Architecture

> **Purpose:** High-level technical structure. Guideline: C4 model (context → containers → components, if necessary).
>
> **Guiding questions:** Which systems does it interact with? What are the deployable units? What technologies are used, and why? What decisions have been documented?

# Core

- [System architecture](architecture/system-architecture)
- [Software architecture](architecture/software-architecture)
- [Design patterns](architecture/design-patterns)
- [Testing](decisions/testing)

## 1. Context
==TBD==
_The system, its users, and the external systems with which it interacts._

```mermaid
flowchart LR
U[User] --> S[System]
S --> E[External system]
```

## 2. Containers / Main Modules
==TBD==

| Container | Responsibility | Technology |
| ---------- | --------------- | ---------- |
| Customer   | | |
| Auth       | | |
| Invoicing  | | |


## 3. Components (only if useful)
_Internal details of a complex container._

## External Integrations
| System | Purpose | Protocol / Contract | Dedicated Note |
| ------- | --------- | -------------------- | ------------- |
| | | | |

## Relevant Decisions

- [ADR-01](adr-001-modular-monolith-as-system-architecture.md)
- [ADR-02](adr-002-clean-and-vertical-slice-as-software-hybrid-architecture.md)
---
_Last reviewed: 2026-10-04_