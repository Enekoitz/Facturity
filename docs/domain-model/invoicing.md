# Domain model: Invoicing

## Invoice (aggregate root)

The aggregate root of the invoicing module. It represents the system's main entity and encapsulates the majority of the complexity.

| Field      | Type            | Rules                                                                                | Nullable |
| ---------- | --------------- | ------------------------------------------------------------------------------------ | -------- |
| Id         | `Guid`          | - Technical identity(persistencia)                                                   | False    |
| TenantId   | `Guid`          | - Owner identity (invoice issuer)                                                    | False    |
| CustomerId | `Guid`          | - Recipient of the invoice (invoice recipient)<br>- Null if is an simplified invoice | True     |
| Header     | `InvoiceHeader` | - VO                                                                                 | False    |
| Data       | `InvoiceData`   | - VO                                                                                 | False    |
| Breakdown  | `TaxBreakdown`  | - VO                                                                                 | False    |
| Status     | `InvoiceStatus` | - Draft / Issued                                                                     | False    |
| Audit      | `AuditInfo`     | - CreatedAt, CreatedBy, UpdatedAt...                                                 | False    |

### Invariants
- `Data.TotalAmount` = sum of bases + amounts de `Breakdown`.
- An `Issued` invoice is inmutable.
- Only a `Draft` invoice can be converte into an `Issued` invoice.
- An invoice must have al least a detail line.

## Value Objects

### InvoiceHeader
| Field        | Type             | Rules                                                     | Nullable |
| ------------ | ---------------- | --------------------------------------------------------- | -------- |
| Series       | `InvoiceSeries`  | - Required<br>- Max length. N                             | No       |
| Number       | `InvoiceNumber`  | - Required<br>- Greater than 0<br>- Related to the series | No       |
| IssuedAtUtc  | `DateTimeOffset` | - Non-future date                                         | No       |
| IsSimplified | `bool`           | - Default: false                                          | No       |

### InvoiceData
| Field            | Type                         | Rules                                   |
| ---------------- | ---------------------------- | --------------------------------------- |
| Description      | `string`                     | - Required, <br>- Max length. N         |
| InvoiceLines     | `IReadOnlyList<InvoiceLine>` | - At least 1 element                    |
| TotalAmount      | `Money`                      | - Positive (except for amended returns) |
| TransactionAtUtc | `DateTimeOffset`             | - Non-future date                       |

### TaxBreakdown
| Field          | Type                           | Rules                  |
| -------------- | ------------------------------ | ---------------------- |
| BreakdownLines | `IReadOnlyList<BreakdownLine>` | - Minimun of 1 element |

### InvoiceLine
| Field       | Type        | Rules                                  |
| ----------- | ----------- | -------------------------------------- |
| Description | `string`    | - Max length: 100                      |
| Units       | `decimal`   |                                        |
| UnitPrice   | `UnitPrice` |                                        |
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

### InvoiceSeries / InvoiceNumber
| Field | Type      | Rules               |
| ----- | --------- | ------------------- |
| Value | `decimal` | - Between 0 and 100 |

Format and length validation. Each series has its own numerical sequence based on the invoices issued within that same series.

# Notes

- All decimal values ​​are rounded away from zero (not banker's rounding).

---
_Last review: 2026-10-10_
