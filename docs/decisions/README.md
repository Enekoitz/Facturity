# Architecture Decision Records (ADRs)

> **Purpose:** To document the *rationale* behind significant decisions. One ADR per decision, numbered.
>
> **Rule:** ADRs are **immutable**. If a decision changes, a new ADR is created to replace the previous one (and the old one is marked as "Superseded").

## When to create one

- Changes in scope.

- Selection of technology, database, pattern, or provider.

- Any decision that is difficult to reverse or likely to be questioned in six months' time.

## How to proceed

1. Copy [`adr-000-template-adr.md`](adr-000-template-adr.md).
2. Assign a number and a name: `adr-014-implementation-of-invoice-signature-algorithm.md`.
3. Add it to the index below.

## Index

| No.                                                                               | Title                                                                              | Status   | Date       |
| --------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------- | -------- | ---------- |
| [001](decisions/adr-001-modular-monolith-as-system-architecture)                  | Adoption of a Modular Monolithic Architecture                                      | Accepted | 2026-01-26 |
| [002](decisions/adr-002-clean-and-vertical-slice-as-software-hybrid-architecture) | Adoption of a Hybrid Software Architecture (Clean Architecture + Vertical Slice) | Accepted | 2026-02-14 |
| [003](decisions/adr-003-identity-jwt-strategy)                                    | Identity, Authentication, and Authorization Strategy                               | Accepted | 2026-10-03 |
| [004](decisions/adr-004-refresh-token-rotation)                                   | Refresh token rotation strategy                                            | Accepted | 2026-10-03 |