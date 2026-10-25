# Glossary

> **Purpose:** Common or ubiquitous language (DDD). Each business term has a single, unique definition. This serves as the foundation for the [domain model](README.md) and the naming of classes within the code.
>
> **Guiding questions:** Are there terms that different people use to mean different things? Which term does the business use, and which one is used in the code?


##Tenant

| Term | Definition | Avoid (ambiguous synonyms in the system) | Codename | TicketBai |
| ------ | -------------------------------------------------- | ---------------------------------------- | -------- | --------- |
| Tenant | Subject who is issuing the invoice to the customer | issuer, organization, issuer | `Tenant` | Issuer |

## Customer

| Term | Definition | Avoid (ambiguous synonyms in the system) | Codename | TicketBai |
| -------- | ------------------------------------------------------------------ | ---------------------------------------- | --------- | ------------ |
| Customer | Person who purchases goods or services then reflected on the invoice | user, account, recipient | `Client` | Recipient |

## Invoicing

| Term | Definition | Avoid (ambiguous synonyms in the system) | Code name | TicketBai (Bizkaia Provincial Council) |
| ---------------- | -------------------------------------- | ----------------------------------------- | ------------------------------------ | ----------------------------------------- |
| Invoice | Invoice entity | | `Invoice`<br>*Pdt. change to Customer | 'Invoice' |
| InvoiceHeader | The header of the invoice | Header | `InvoiceHeader` | 'InvoiceHeader' |
| InvoiceData | The data of the invoice | Header | `InvoiceHeader` | 'InvoiceData' |
| BreakDownline | The tax breakdown lines of the invoice | | | 'BreakdownType' |
| TransactionAtUtc | The transaction datetime | | `TransactionAtUtc` | 'OperationDate' |
| IssuedAtUtc | The issue datetime | | `IssuedAtUtc`                        | 'FechaExpedicion' and<br>'HoraExpedicion' |
| Series           | | | | |

- In TicketBai, `Factura` refers to one of the components of the 'operation registration file' (*Fichero de alta de operación*).
---
_Last reviewed: 2026-10-17_