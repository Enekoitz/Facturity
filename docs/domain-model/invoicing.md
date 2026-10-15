# Domain model: Invoicing

## Invoice (aggregate root)

The aggregate root of the invoicing module. It represents the system's main entity and encapsulates the majority of the complexity.

| Field        | Type                           | Rules                                                                                | Nullable |
| ------------ | ------------------------------ | ------------------------------------------------------------------------------------ | -------- |
| Id           | `Guid`                         | - Technical identity(persistencia)                                                   | No       |
| TenantId     | `Guid`                         | - Owner identity (invoice issuer)                                                    | No       |
| CustomerId   | `Guid`                         | - Recipient of the invoice (invoice recipient)<br>- Null if is an simplified invoice | Yes      |
| Header       | `InvoiceHeader`                | - VO                                                                                 | No       |
| Data         | `InvoiceData`                  | - VO                                                                                 | No       |
| TaxBreakdown | `IReadOnlyList<BreakdownLine>` | - Minimun of 1 element                                                               | No       |
| Status       | `InvoiceStatus`                | - Draft / Issued                                                                     | No       |
| Audit        | `AuditInfo`                    | - CreatedAt, CreatedBy, UpdatedAt...                                                 | No       |

### Invariants
- `Data.TotalAmount` = sum of bases + amounts of `InvoiceLine` list.
- An `Issued` invoice is inmutable.
- Only a `Draft` invoice can be converted into an `Issued` invoice.
- An invoice must have at least a detail line.
- The InvoiceHeader field number is calculated and assigned solely during the issuance process, via a database transaction. This ensures the integrity and sequential continuity of the numbers—with no gaps—while also preventing potential race conditions and concurrency conflicts.
- `Header.IssuedAtUtc` cannot be null or future when the invoice is being issued.

## Entities

### InvoiceSeries
| Field | Type     | Rules            |
| ----- | -------- | ---------------- |
| Id    | `Guid`   |                  |
| Name  | `string` | - Max length: 20 

## Value Objects

### InvoiceHeader
| Field        | Type                   | Rules                                       | Nullable |
| ------------ | ---------------------- | ------------------------------------------- | -------- |
| SeriesId     | `Guid` (or `SeriesId`) | - Required<br>- Max length. 20              | No       |
| Number       | `uint`                 | - Greater than 0<br>- Related to the series | Yes      |
| IssuedAtUtc  | `DateTimeOffset`       | - Non-future date                           | Yes      |
| IsSimplified | `bool`                 | - Default: false                            | No       |
- The "Number" field is only set in the database transaction when the invoice is issued. "InvoiceHeader" can only obtain this value via an invoice that has been previously loaded and issued using EF Core.

### InvoiceData
| Field            | Type                         | Rules                                   |
| ---------------- | ---------------------------- | --------------------------------------- |
| Description      | `string`                     | - Required, <br>- Max length. 500       |
| InvoiceLines     | `IReadOnlyList<InvoiceLine>` | - At least 1 element                    |
| TotalAmount      | `Money`                      | - Positive (except for amended returns) |
| TransactionAtUtc | `DateTimeOffset`             | - Non-future date                       |

### InvoiceLine
| Field       | Type        | Rules                                  |
| ----------- | ----------- | -------------------------------------- |
| Description | `string`    | - Max length: 100                      |
| Units       | `decimal`   | - Greater than 0                       |
| UnitPrice   | `UnitPrice` | - Max six decimal places               |
| TaxBase     | `Money`     | - = Units * UnitAmount                 |
| TaxRate     | `TaxRate`   | - 0 to 100                             |
| TaxAmount   | `Money`     | - = TaxBase * TaxRate / 100 (rounding) |
| TotalAmount | `Money`     | - = TaxBase + TaxAmount                |

### BreakdownLine
| Field       | Type      | Rules                                  |
| ----------- | --------- | -------------------------------------- |
| TaxBase     | `Money`   | - = Units * UnitAmount                 |
| TaxRate     | `TaxRate` | - 0 a 100                              |
| TaxAmount   | `Money`   | - = TaxBase * TaxRate / 100 (rounding) |
| TotalAmount | `Money`   | - = TaxBase + TaxAmount                |
- Deriva de las `invoice lines`.

### Money
| Field    | Type      | Rules                                        |
| -------- | --------- | -------------------------------------------- |
| Amount   | `decimal` | - Max. two decimal places                    |
| Currency | `char`    | - Operations only between the same currency. |
- It accepts a maximum of two decimal places.

### UnitPrice
| Field    | Type      | Rules                                        |
| -------- | --------- | -------------------------------------------- |
| Price    | `decimal` | - Max. six decimal places                    |
| Currency | `char`    | - Operations only between the same currency. |
- It accepts a maximum of six decimal places.

### TaxRate
| Field | Type      | Rules               |
| ----- | --------- | ------------------- |
| Value | `decimal` | - Between 0 and 100 |

## Enums

### InvoiceStatus
| Element  | Value | Description                                                        |
| -------- | ----- | ------------------------------------------------------------------ |
| `Draft`  | 1     | The invoice is in draft status, not issued yet                     |
| `Issued` | 2     | The invoice has been issued previously passing through draf status |

# Notes

- All decimal values ​​are rounded away from zero (not banker's rounding).

---
_Last review: 2026-10-10_
