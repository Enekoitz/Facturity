# Invoicing Runbook: VAT, Invoice Types, and TicketBAI

Reference document for modeling the invoicing domain. **Status: draft pending validation.** This document is based on general knowledge and standard practices; specific amounts, deadlines, codes, and field names should be verified against official sources (the Invoicing Regulations, the VAT Act—or the relevant regional *norma foral*—and TicketBAI technical specifications). Points of uncertainty are marked with **[VERIFY]**.

## 1. Scope

- **Purpose:** To establish a common vocabulary and business rules for the invoicing module.
- **Project Scope (v1):** Invoices issued to domestic clients for transactions that are subject to (and not exempt from) VAT; includes both standard and simplified invoices, covering one or more VAT rates.
- **Out of Scope (v1):** Exempt or non-subject transactions, the "equivalence surcharge" (*recargo de equivalencia*), reverse charge mechanisms, special tax regimes, foreign clients, corrective invoices, digital fingerprinting, electronic signatures, and submission processes.
- The remainder of this document outlines the broader landscape to ensure the model remains flexible and extensible.

## 2. Regulatory Framework (for reference)

| Scope               | Regulation                                                | Purpose                                                               |
| ------------------- | --------------------------------------------------------- | --------------------------------------------------------------------- |
| National            | VAT Act (Law 37/1992)                                     | Defines taxable/exempt transactions, VAT rates, and corrections       |
| National            | Invoicing Regulations (Royal Decree 1619/2012)            | Defines who must issue invoices, required content, deadlines, and rules for simplified/corrective invoices |
| Regional (*Foral*)  | Regional VAT Laws (*Normas Forales*) for Bizkaia, Gipuzkoa, and Álava | Equivalent to national law, with specific regional nuances            |
| Regional (*Foral*)  | TicketBAI Regional Decrees and Technical Specifications   | Software requirements, XML schema, digital signatures, fingerprinting, and submission | Note: In the Basque Country, VAT is managed by the regional tax authorities (*haciendas forales*), with regulations that are closely aligned with national (state-level) legislation. It is advisable to verify specific details—such as deadlines, tax regimes, and classification codes—against the regional regulations applicable to your specific territory. **[VERIFY]**

## 3. Basic Concepts

| Concept            | Suggested Code    | Definition                                                         |
| ------------------ | ----------------- | ------------------------------------------------------------------ |
| Tax base           | `TaxBase`         | The amount upon which the tax is calculated, excluding VAT         |
| Tax rate           | `TaxRate`         | The applicable VAT percentage                                      |
| Tax amount         | `TaxAmount`       | The tax base multiplied by the tax rate (in euros)                 |
| Total              | `TotalAmount`     | The tax base plus the tax amount (minus any withholdings, if applicable, for payment purposes) |
| Issuer             | `Issuer` (tenant) | The party issuing the invoice                                      |
| Recipient          | `Customer`        | The party receiving the invoice                                    |
| Series             | `InvoiceSeries`   | A numbered grouping of invoices                                    |
| Number             | `InvoiceNumber`   | The sequential position within a given series                      |
| Breakdown          | `BreakdownLine`   | A tax summary categorized by tax rate                              |
| Detail line        | `InvoiceLine`     | An individual item or service being invoiced                       |

Formulas for a single line:

- Tax base = units multiplied by unit price, rounded to 2 decimal places.
- Tax amount = tax base multiplied by tax rate / 100, rounded to 2 decimal places.
- Total = tax base plus tax amount.

Example: A tax base of 100.00 at a 21% rate results in a tax amount of 21.00 and a total of 121.00.

## 4. Types of Invoices

### 4.1 Full (Ordinary) Invoice

Contains the full identification details of both the issuer and the recipient. This type of invoice is mandatory when the recipient is a business owner or professional who needs it to deduct VAT, or when the client requests it for tax purposes.

Standard required content **[VERIFY]**:

- Number and, where applicable, series (sequential within the series).
- Date of issuance.
- Date of the transaction, if different from the date of issuance.
- Name or legal business name, tax identification number (NIF), and address of both the issuer and the recipient.
- Description of the transactions, including the information necessary to determine the tax base.
- Applicable tax rate(s).
- Tax amount, shown separately.
- Tax base, broken down by tax rate.
- If the transaction is exempt or subject to the reverse charge mechanism, a reference to the relevant legal provision or an equivalent statement.

### 4.2 Simplified invoice (receipt)

This is issued without identifying the recipient. It is typically used for retail sales.

- It may be issued, among other cases, when the total amount does not exceed **€400 (VAT included)**, or **€3,000 (VAT included)** for certain types of activities (for example, retail sales).