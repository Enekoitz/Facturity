# Domain model: Invoicing

## Invoice (aggregate root)

The aggregate root of the invoicing module. It represents the system's main entity and encapsulates the majority of the complexity.

| Field      | Type            | Rules                                                                                | Nullable |
| ---------- | --------------- | ------------------------------------------------------------------------------------ | -------- |
| Id         | `Guid`          | Technical identity(persistencia)                                                     | False    |
| TenantId   | `Guid`          | Owner identity (invoice issuer)                                                      | False    |
| CustomerId | `Guid`          | - Recipient of the invoice (invoice recipient)<br>- Null if is an simplified invoice | True     |
| Header     | `InvoiceHeader` | VO                                                                                   | False    |
| Data       | `InvoiceData`   | VO                                                                                   | False    |
| Breakdown  | `TaxBreakdown`  | VO                                                                                   | False    |
| Status     | `InvoiceStatus` | Draft / Issued                                                                       | False    |
| Audit      | `AuditInfo`     | CreatedAt, CreatedBy, UpdatedAt...                                                   | False    |

### Invariantes
- `Data.TotalAmount` = sum of bases + amounts de `Breakdown`.
- An `Issued` invoice is inmutable.
- Only a `Draft` invoice can be converte into an `Issued` invoice.
- An invoice must have al least a detail line. 

## Value Objects: Primary

### InvoiceHeader
| Field        | Type             | Rules                                | Nullable |
| ------------ | ---------------- | ------------------------------------ | -------- |
| Series       | `InvoiceSeries`  | Required, max length. N              | No       |
| Number       | `InvoiceNumber`  | Required, > 0, related to the series | No       |
| IssuedAtUtc  | `DateTimeOffset` | Non-future date                      | No       |
| IsSimplified | `bool`           |                                      | No       |

### InvoiceData
| Field            | Type             | Rules                                 |
| ---------------- | ---------------- | ------------------------------------- |
| Description      | `string`         | Required, max length. N               |
| TotalAmount      | `Money`          | Positive (except for amended returns) |
| TransactionAtUtc | `DateTimeOffset` | Non-future date                       |

### TaxBreakdown
| Field | Type                           | Rules                |
| ----- | ------------------------------ | -------------------- |
| Lines | `IReadOnlyList<BreakdownLine>` | Minimun of 1 element |


## Value Objects: Secondary

### BreakdownLine
| Field       | Type      | Rules                          |
| ----------- | --------- | ------------------------------ |
| Description | `string`  | Max length: 255                |
| Quantity    | `decimal` |                                |
| Units       | `decimal` |                                |
| TaxBase     | `Money`   |                                |
| TaxRate     | `TaxRate` | 0 a 100                        |
| TaxAmount   | `Money`   | = TaxBase * TaxRate (rounding) |

### Money
| Field    | Type      | Rules                                      |
| -------- | --------- | ------------------------------------------ |
| Amount   | `decimal` | Two decimal places                         |
| Currency | `char`    | Operations only between the same currency. |

### TaxRate
| Field | Type      | Rules               |
| ----- | --------- | ------------------- |
| Value | `decimal` | - Between 0 and 100 |

### InvoiceSeries / InvoiceNumber
Format and length validation. Each series has its own numerical sequence based on the invoices issued within that same series.

---
_Last review: 2026-10-10_
