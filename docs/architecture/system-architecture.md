# System Architecture: **Evolvable Modular Monolith**.

The evolvable modular monolith sits at the midpoint between the complexity of microservices and the disarray of a traditional monolith. It enables the creation of a system that is relatively easy to maintain, while also allowing for the future extraction of specific modules into microservices.

## Why?

A modular monolith allows for the logical division of a system into distinct modules ("bounded contexts"). These are isolated into separate assemblies, with various techniques and technologies employed to facilitate communication between them. This ensures high system cohesion, enforces strict encapsulation, and minimizes coupling between modules.

Inter-modular communication is asynchronous by default to maintain temporal decoupling.

At the physical level, it remains a single deployment unit, running as a single process. This greatly simplifies deployment procedures and reduces the network and infrastructure overhead typically associated with microservices.

Furthermore, it allows each module to implement its own specific software architecture, rather than requiring the entire application to adhere to a single, uniform architectural pattern.

>NOTE: The software architecture for each module is defined on the following page: [[Software Architecture]]

Should the decision be made in the future to refactor the project into a microservices-based architecture, the transition from a modular monolith is more natural and significantly less complex.

### Architectural Diagrams

==TBD==

### Main Components

| Module        | Description                         | Technology         | Responsibility                                               |
| ------------- | ----------------------------------- | ------------------ | ------------------------------------------------------------- |
==TBD==

## Notes
None at present

---
_Last reviewed: 2026-10-04_