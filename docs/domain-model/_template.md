# Domain model: MODULE_NAME

_Optional description_

## AGGREGATE_ROOT_ENTITY (aggregate root)

The aggregate root of the invoicing module.

| Field        | Type                           | Rules                                                                                | Nullable |
| ------------ | ------------------------------ | ------------------------------------------------------------------------------------ | -------- |
| Id           | `Guid`                         | - Technical identity                                                                 | No       |
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

Regarding the operation type:

- Create Draft:
	- The `Status` must be set to `Draft` and not to `Issued`.
	- An uncompleted invoice can be saved as a draft; final validation takes place during the issuance process. So:
		- It can accept zero or null invoice lines.
		- It can save `CustomerId` to null when is specified as a no simplified invoice.
		
- Update Draft (without issuing):
	- The `Status` must be set to `Draft` and not to `Issued`.
	- An uncompleted invoice can be saved as a draft; final validation takes place during the issuance process. So:
		- It can accept zero or null invoice lines.
		- It can save `CustomerId` to null when is specified as a no simplified invoice.
		
- Issue:
	- The  `Header.Number` is calculated and assigned solely during the issuance process, via issuing database transaction. This ensures the integrity and sequential continuity of the numbers—with no gaps—while also preventing potential race conditions and concurrency conflicts.
	- The `Status` must be `Draft` and not `Issued`.
	- The `Header.IssuedAtUtc` cannot be null or future.
	- If is a simplified invoice (`HeaderIsSimplified == true`), `CustomerId` must be null.
	- If is not a simplified invoice (`HeaderIsSimplified == false`), `CustomerId` must be valid and not null.
	- The invoice must have at least a detail line. And, consequently, at least a breakdown line.

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
_Last review: 2026-10-17_
