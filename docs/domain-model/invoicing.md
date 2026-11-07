# Domain model: Invoicing

## Aggregate root
### Invoice

| Field            | Type                           | Rules                                                                                | Nullable                                           |
| ---------------- | ------------------------------ | ------------------------------------------------------------------------------------ | -------------------------------------------------- |
| Id               | `Guid`                         | - Technical identity                                                                 | No                                                 |
| TenantId         | `Guid`                         | - Owner identity (invoice issuer)                                                    | No                                                 |
| CustomerId       | `Guid`                         | - Recipient of the invoice (invoice recipient)<br>- Null if is an simplified invoice | Yes                                                |
| SeriesId         | `Guid`                         |                                                                                      | Yes                                                |
| Status           | `InvoiceStatus`                | - Draft / Issued                                                                     | No                                                 |
| IssuanceInfo     | `IssuanceInfo`                 | - VO                                                                                 | Yes                                                |
| IsSimplified     | `bool`                         | - Default: false                                                                     | No                                                 |
| Description      | `string`                       | - Required <br>- Max length. 250                                                     | Yes                                                |
| InvoiceLines     | `IReadOnlyList<InvoiceLine>`   | - At least 1 element                                                                 | No (but can exist as an empty collection in Draft) |
| TaxBreakdown     | `IReadOnlyList<BreakdownLine>` | - Minimun of 1 element                                                               | No                                                 |
| TotalAmount      | `Money`                        | - Positive (except for amended returns)                                              | No                                                 |
| TransactionAtUtc | `DateTimeOffset`               | - Non-future date                                                                    | Yes                                                |
| Currency         | `string`                       | - Operations only between the same currency                                          | No                                                 |

### Invariants

- `TotalAmount` = sum of bases + amounts of `InvoiceLine` list.
- An `Issued` invoice is inmutable.
- Only a `Draft` invoice can be converted into an `Issued` invoice.

Regarding the type of operation:

- Creating an invoice draft:
	- The `Status` must be set to `Draft`, not `Issued`.
	- An incomplete invoice may be saved as a draft; final validation occurs during the issuance process. Therefore:
		- An invoice identifier cannot be saved with the value `Guid.Empty`.
		- An empty collection of invoice lines (zero elements) is permitted, but a null collection is not.
	- Values ​​such as the description, transaction date, series, or customer are defined via additional operations (methods).
	
- Changing the customer:
	- This is only possible when the invoice is in the `Draft` state.
	- The customer identifier cannot be `Guid.Empty`.
	- It may be set to null.
	- It is possible to assign a null value to `CustomerId` when the invoice is not of the "simplified" type; however, this action is restricted during the issuance process.
	- It is not possible to assign a non-null value to `CustomerId` for a simplified invoice.
	
- Changing the currency:
	- This is only possible when the invoice is in the `Draft` state.
	- The currency code must be three characters long.
	
- Changing the description:
	- This is only possible when the invoice is in the `Draft` state.
	- The description may be null; otherwise, it must not exceed the maximum permitted length.
	
- Changing the transaction date:
	- This is only possible when the invoice is in the `Draft` state.
	- The transaction date cannot be a future date.
	
- Changing the series:
	- This is only possible when the invoice is in the `Draft` state.
	- The series identifier cannot be `Guid.Empty`.
	- It may be set to null. 
	- Issuance of an invoice draft:
	
- The `IssuanceInfo.Number` field is calculated and assigned exclusively during the issuance process, via a database transaction. This ensures the integrity and sequential continuity of the numbers—with no gaps—while preventing potential race conditions and concurrency conflicts.
	- The `Status` must be `Draft`, not `Issued`.
	- The `IssuanceInfo.IssuedAtUtc` field cannot be null, nor can it represent a future date.
	- If it is a simplified invoice (`IsSimplified == true`), `CustomerId` must be null.
	- If it is not a simplified invoice (`IsSimplified == false`), `CustomerId` must be valid and non-null.
	- The invoice must contain at least one invoice line item.
## Entities

### InvoiceSeries
| Field       | Type       | Rules             | Nullable |
| ----------- | ---------- | ----------------- | -------- |
| Id          | `Guid`     |                   | No       |
| TenantId    | `TenantId` |                   | No       |
| Name        | `string`   | - Max length: 20  | No       |
| Description | `string`   | - Max length: 250 | Yes      |
### InvoiceLine
| Field       | Type        | Rules                                  | Nullable |
| ----------- | ----------- | -------------------------------------- | -------- |
| Id          | `Guid`      |                                        | No       |
| TenantId    | `Guid`      |                                        | No       |
| InvoiceId   | `Guid`      | -                                      | No       |
| Description | `string`    | - Max length: 100                      | No       |
| Units       | `decimal`   | - Greater than 0                       | No       |
| UnitPrice   | `UnitPrice` | - Max six decimal places               | No       |
| TaxBase     | `Money`     | - = Units * UnitAmount                 | No       |
| TaxRate     | `TaxRate`   | - 0 to 100                             | No       |
| TaxAmount   | `Money`     | - = TaxBase * TaxRate / 100 (rounding) | No       |
| TotalAmount | `Money`     | - = TaxBase + TaxAmount                | No       |

## Value Objects

### IssuanceInfo
| Field       | Type             | Rules                                                                | Nullable |
| ----------- | ---------------- | -------------------------------------------------------------------- | -------- |
| Number      | `ulong`          | - Greater than 0<br>- Related to the series<br>- Only set by EF Core | Yes      |
| IssuedAtUtc | `DateTimeOffset` | - Non-future date                                                    | No       |

### BreakdownLine
| Field       | Type      | Rules                                  |
| ----------- | --------- | -------------------------------------- |
| TaxBase     | `Money`   | - = Units * UnitAmount                 |
| TaxRate     | `TaxRate` | - 0 a 100                              |
| TaxAmount   | `Money`   | - = TaxBase * TaxRate / 100 (rounding) |
| TotalAmount | `Money`   | - = TaxBase + TaxAmount                |
- Deriva de las `invoice lines`.

### Money
| Field  | Type      | Rules                    |
| ------ | --------- | ------------------------ |
| Amount | `decimal` | - Max. decimal places: 2 |
- It accepts a maximum of two decimal places.

### UnitPrice
| Field | Type      | Rules                    |
| ----- | --------- | ------------------------ |
| Price | `decimal` | - Max. decimal places: 6 |

- It accepts a maximum of six decimal places.

### TaxRate
| Field | Type      | Rules                                           |
| ----- | --------- | ----------------------------------------------- |
| Value | `decimal` | - Between 0 and 100<br>- Max. decimal places: 2 |

## Enums

### InvoiceStatus
| Element  | Value | Description                                                        |
| -------- | ----- | ------------------------------------------------------------------ |
| `Draft`  | 1     | The invoice is in draft status, not issued yet                     |
| `Issued` | 2     | The invoice has been issued previously passing through draf status |
- Disclaimer: It is important to consider whether the numbering sequence truly needs to be modified, and to exercise caution, as this directly affects invoice control and stored records.

# Notes

- The domain layer does not verify whether the `CustomerId` or `SeriesId` identifiers belong to the specified **tenant**. Instead, this is ensured through validation within the use cases and by implementing robust, critical infrastructure security mechanisms—such as global filters and row-level security (RLS) at the database level. Further details will be provided in the next ADR: [[adr-005-tenant-security-warranty]].

- **All decimal values ​​are rounded away from zero (not banker's rounding)**.

---
_Last review: 2026-10-31_
