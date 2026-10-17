# Glossary

> **Purpose:** Common or ubiquitous language (DDD). Each business term has a single, unique definition. This serves as the foundation for the [domain model](README.md) and the naming of classes within the code.
>
> **Guiding questions:** Are there terms that different people use to mean different things? Which term does the business use, and which one is used in the code?


## Tenant 

| Term   | Definition                                         | Avoid (ambiguous synonyms in the system) | Codename | TicketBai |
| ------ | -------------------------------------------------- | ---------------------------------------- | -------- | --------- |
| Tenant | Subject who is issuing the invoice to the customer | emisor, organizacion, issuer             | `Tenant` | Emisor    |

## Customer 

| Term     | Definition                                                         | Avoid (ambiguous synonyms in the system) | Codename  | TicketBai    |
| -------- | ------------------------------------------------------------------ | ---------------------------------------- | --------- | ------------ |
| Customer | Persona que compra bienes o servicios luego plasmado en la factura | usuario, cuenta, destinatario            | `Cliente` | Destinatario |


## Invoicing 

| Término          | Definición                             | Evitar (sinónimos ambiguos en el sistema) | Nombre en código                     | TicketBai (Diputación Foral de Bizkaia)   |
| ---------------- | -------------------------------------- | ----------------------------------------- | ------------------------------------ | ----------------------------------------- |
| Invoice          | Invoice entity                         |                                           | `Invoice`<br>*Pdt. cambia a Customer | 'Factura'                                 |
| InvoiceHeader    | The header of the invoice              | Cabecera                                  | `InvoiceHeader`                      | 'CabeceraFactura'                         |
| InvoiceData      | The data of the invoice                | Cabecera                                  | `InvoiceHeader`                      | 'DatosFactura'                            |
| BreakDownline    | The tax breakdown lines of the invoice |                                           |                                      | 'TipoDesglose'                            |
| TransactionAtUtc | The transaction datetime               |                                           | `TransactionAtUtc`                   | 'FechaOperacion'                          |
| IssuedAtUtc      | The issue datetime                     |                                           | `IssuedAtUtc`                        | 'FechaExpedicion' and<br>'HoraExpedicion' |
| Series           |                                        |                                           |                                      |                                           |

- En TicketBai, `Factura` hace referencia a una de las partes del 'Fichero de alta de operación'.
---
_Última revisión: 2026-10-17_
