# Architecture Decision: CQRS with Generic Interfaces, Without the Mediator Pattern

## Context

This project implements an electronic invoicing system (TicketBai) using ASP.NET Core Minimal APIs. The domain encompasses use cases with non-trivial business rules, including: the creation and chaining of invoices via hashing; digital signatures; electronic submission to the regional tax authority (Hacienda Foral); invoice cancellation; management of invoice series; and tax calculations (VAT, surcharge for equivalence).

Given this context, an architecture was required that would clearly separate write operations from read operations, keep each use case isolated and testable, and prevent the emergence of a monolithic "application service" with an excessive number of responsibilities. The decision was made to implement **CQRS** using **generic command/query interfaces**, without employing the **Mediator pattern** (neither a custom implementation nor one provided by libraries such as MediatR).

## What CQRS Is (and Isn't)

CQRS (Command Query Responsibility Segregation) is the principle of separating the model that **writes** state from the model that **reads** state. It does not mandate the use of any specific library or design pattern; it can be achieved using standard service classes, provided that write and read methods are clearly distinguished—ideally returning different types (with write operations acting upon the domain, and read operations returning DTOs optimized for querying).

It is important to distinguish CQRS from the **Mediator pattern**—a distinction often blurred due to the popularity of MediatR within the .NET ecosystem. The Mediator pattern is merely one possible implementation technique for decoupling the component that issues a command from the component that processes it. It is entirely possible to have CQRS without a Mediator, and a Mediator without CQRS. ## The chosen structure: generic command/query interfaces

```csharp
public interface ICommand<TResult> { }

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{ 
Task<TResult> HandleAsync(TCommand command, CancellationToken ct);
}

public interface IQuery<TResult> { }

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{ 
Task<TResult> HandleAsync(TQuery query, CancellationToken ct);
}
```

Each domain use case is modeled as a separate command/handler (or query/handler) pair:

```csharp
public record CreateInvoiceCommand(Guid CustomerId, IReadOnlyList<InvoiceLine> Lines, string SeriesCode) 
: ICommand<Guid>;

public class CreateInvoiceHandler : ICommandHandler<CreateInvoiceCommand, Guid>
{ 
private readonly InvoiceDbContext _db; 
private readonly IInvoiceChainService _chainService; 

public CreateInvoiceHandler(InvoiceDbContext db, IInvoiceChainService chainService) 
{ 
_db = db; 
_chainService = chainService; 
} 

public async Task<Guid> HandleAsync(CreateInvoiceCommand command, CancellationToken ct) 
{ 
var previousHash = await _chainService.GetLastHashAsync(command.SeriesCode, ct); 
var invoice = Invoice.Create(command.CustomerId, command.Lines, previousHash); 

_db.Invoices.Add(invoice); 
await _db.SaveChangesAsync(ct); 

return invoice.Id; 
}
}
```

Endpoints inject the specific handler they require directly, without passing through any intermediate dispatcher:

```csharp
app.MapPost("/invoices", async (
CreateInvoiceRequest request,
ICommandHandler<CreateInvoiceCommand, Guid> handler,
CancellationToken ct) =>
{
var command = new CreateInvoiceCommand(request.CustomerId, request.Lines, request.SeriesCode);
var invoiceId = await handler.HandleAsync(command, ct);
return Results.Created($"/invoices/{invoiceId}", invoiceId);
});
```

## Why generic interfaces provide real value

**Isolation by use case.** Each handler depends only on the collaborators required for its specific use case. `SignInvoiceHandler` depends on the XAdES signing service and the digital certificate; `SubmitToTicketBaiHandler` depends on an HTTP client for communicating with the regional tax authority; `GetInvoiceByIdHandler` depends solely on a read-only query. None of them carry dependencies unrelated to their specific responsibility—unlike a monolithic `InvoiceService` containing twenty methods, where all those dependencies would be injected at once.

**Testability.** Each handler can be unit-tested by mocking only two or three collaborators, rather than having to set up the entire state of a multi-responsibility service just to test a single method.

**Explicit contracts.** `CreateInvoiceCommand` is a type with its own distinct identity: it can be logged, versioned (e.g., `CreateInvoiceCommandV2`), or reused as a message—for instance, if a queue or an "outbox" pattern is later introduced for asynchronous submission to the regional tax authority. A conventional service method lacks this identity and flexibility.

**A hook for cross-cutting concerns, without the need for a mediator.** When