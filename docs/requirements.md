# Requirements

> **Purpose:** What the system must fulfill, prioritized.
>
> **Guiding questions:** What must the system do? With what level of quality (performance, security, accessibility)? What happens if this requirement is missing?
>
> **Priority (MoSCoW):** **M**ust (essential) · **S**hould (important) · **C**ould (desirable) · **W**on't (not for now).

## Functional

| ID     | Requirement                                                                                                                                                                                                                                   | Priority | Origin               | User Story |
| ------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------- | -------------------- | ---------- |
| RF-001 | _The system must allow users to register and log in using credentials._                                                                                                                                                                      | M         | _Functional analysis_ | TBD        |
| RF-002 | _Users must be able to manage the clients to whom invoices are assigned: create, update data, and remove from the system._                                                                                                                    | M         | _Functional analysis_ | TBD        |
| RF-003 | _Users must be able to manage series: create and deactivate/activate._                                                                                                                                                                      | M         | _Functional analysis_ | TBD        |
| RF-004 | _It must be possible to add specific preferences to clients (VAT type, "equivalence surcharge" type, etc.)._                                                                                                                                  | C         | _Functional analysis_ | TBD        |
| RF-005 | _Users must be able to create an invoice manually, with the option to save it as a "draft" or send it directly to the relevant tax authority._                                                                                                | M         | _Functional analysis_ | TBD        |
| RF-006 | _Users must be able to delete an invoice, provided it has not yet been sent to the tax authority._                                                                                                                                          | M         | _Functional analysis_ | TBD        |
| RF-007 | | RF-007 | _It must be possible to create an invoice and send it immediately to the relevant tax authority, ensuring compliance with signature validation and QR code generation requirements._ | S | _Functional analysis_ | Pending |
| RF-008 | _It must be possible to void an invoice that has already been sent to the tax authority, though it cannot be deleted._ | S | _Functional analysis_ | Pending |
| RF-009 | _The system must reliably manage and assign invoice numbers when submitting to the tax authority (implementing concurrency control to ensure no invoice number is ever reused)._ | S | _Functional analysis_ | Pending |
| RF-010 | _It must be possible to submit multiple invoices to the tax authority in a single operation, provided they are in a valid state._ | S | _Functional analysis_ | Pending |
| RF-011 | _As a multi-tenant system, it must assign a new tenant to each registered user._ | M | _Functional analysis_ | Pending |
| RF-012 | _As a multi-tenant system, it must provide an option to link to an existing tenant (organization) via an invitation code or similar mechanism._ | W | _Functional analysis_ | Pending |
| RF-013 | _The system must require that a user belongs to a valid, active tenant—with appropriate tax information—in order to generate invoices. Upon user registration, if a new tenant is being created, the corresponding tenant details must be provided._ | M | _Functional analysis_ | Pending |
| RF-014 | _Users must be able to view the details of a specific invoice, regardless of its current status._ | S | _Functional analysis_ | Pending |