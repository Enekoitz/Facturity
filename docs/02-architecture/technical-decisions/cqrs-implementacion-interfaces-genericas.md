# Decisión de arquitectura: CQRS con interfaces genéricas, sin patrón Mediador

## Contexto

Este proyecto implementa un sistema de facturación electrónica (TicketBai) sobre ASP.NET Core Minimal APIs. El dominio incluye casos de uso con reglas de negocio no triviales: creación y encadenamiento de facturas mediante hash, firma digital, envío telemático a la Hacienda Foral, anulación de facturas, gestión de series de facturación y cálculo de impuestos (IVA, recargo de equivalencia).

Dado ese contexto, se necesitaba una arquitectura que separase claramente las operaciones de escritura de las de lectura, mantuviese cada caso de uso aislado y testeable, y evitara la aparición de un "servicio de aplicación" monolítico con demasiadas responsabilidades. Se decidió aplicar **CQRS** mediante **interfaces genéricas de comando/query**, sin recurrir a un patrón **Mediador** (ni casero ni a través de librerías como MediatR).

## Qué es CQRS y qué no es

CQRS (_Command Query Responsibility Segregation_) es el principio de separar el modelo que **escribe** estado del modelo que **lee** estado. No exige ninguna librería ni ningún patrón de diseño concreto para implementarse: se puede lograr con clases de servicio normales, siempre que los métodos de escritura y lectura estén claramente diferenciados y, idealmente, devuelvan tipos distintos (el de escritura opera sobre el dominio; el de lectura devuelve DTOs optimizados para consulta).

Es importante distinguir CQRS del **patrón Mediador**, con el que suele confundirse por la popularidad de MediatR en el ecosistema .NET. El Mediador es únicamente una técnica de implementación —una entre varias posibles— para desacoplar quién emite un comando de quién lo procesa. Se puede tener CQRS sin Mediador, y Mediador sin CQRS.

## La estructura elegida: interfaces genéricas de comando/query

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

Cada caso de uso del dominio se modela como un par comando/handler (o query/handler) independiente:

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

Los endpoints inyectan directamente el handler concreto que necesitan, sin pasar por ningún dispatcher intermedio:

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

## Por qué las interfaces genéricas sí aportan valor

**Aislamiento por caso de uso.** Cada handler depende únicamente de los colaboradores que su caso de uso concreto necesita. `SignInvoiceHandler` depende del servicio de firma XAdES y del certificado digital; `SubmitToTicketBaiHandler` depende del cliente HTTP hacia la Hacienda Foral; `GetInvoiceByIdHandler` solo depende de una consulta de solo lectura. Ninguno arrastra dependencias ajenas a su responsabilidad, a diferencia de lo que ocurriría en un `InvoiceService` único con veinte métodos y todas esas dependencias inyectadas de golpe.

**Testabilidad.** Cada handler se puede testear de forma unitaria mockeando solo dos o tres colaboradores, en lugar de tener que preparar el estado completo de un servicio con muchas responsabilidades para probar un único método.

**Contratos explícitos.** `CreateInvoiceCommand` es un tipo con identidad propia: se puede loguear, versionar (`CreateInvoiceCommandV2`), o reutilizar como mensaje si en el futuro se introduce una cola o un patrón _outbox_ para el envío asíncrono a la Hacienda Foral. Un método de servicio convencional no tiene esa identidad ni esa flexibilidad.

**Punto de enganche para comportamiento transversal, sin necesidad de mediador.** Cuando se necesita validación uniforme (NIF válido, importes cuadrando, series correlativas), basta con envolver los handlers en un _decorator_ genérico registrado en el contenedor de DI (por ejemplo, con Scrutor), sin necesidad de un dispatcher central:

```csharp
public class ValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;
    private readonly IValidator<TCommand> _validator;

    public ValidationDecorator(ICommandHandler<TCommand, TResult> inner, IValidator<TCommand> validator)
    {
        _inner = inner;
        _validator = validator;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken ct)
    {
        var result = await _validator.ValidateAsync(command, ct);
        if (!result.IsValid) throw new ValidationException(result.Errors);

        return await _inner.HandleAsync(command, ct);
    }
}
```

Este decorator se aplica automáticamente a todos los comandos registrados, obteniendo el mismo beneficio de "comportamiento transversal uniforme" que ofrecen los _pipeline behaviors_ de MediatR, sin necesitar la librería ni la capa de resolución dinámica que implica un mediador.

## Por qué se descarta el patrón Mediador (casero o MediatR)

El patrón Mediador introduce una capa de indirección adicional: en vez de que el endpoint conozca e inyecte el handler concreto, conoce únicamente un dispatcher genérico (`IMediator`, `ISender`) que resuelve en tiempo de ejecución —normalmente mediante reflexión— qué handler corresponde a cada comando. Ese coste solo se amortiza cuando:

- El número de comandos y queries es grande (decenas), y
- Se necesita aplicar comportamiento transversal de forma completamente uniforme a través de un único punto de entrada, y
- Se prioriza la uniformidad de la firma de los endpoints (todos inyectan `IMediator`) sobre la trazabilidad directa de qué endpoint usa qué handler.

En este proyecto ninguna de estas condiciones se cumple con suficiente peso:

- El número de casos de uso es reducido (en torno a diez o quince: creación, anulación, firma, envío telemático, consultas), por lo que el problema de escala que resuelve el mediador no llega a manifestarse.
- El comportamiento transversal necesario (validación, y potencialmente manejo transaccional) ya se resuelve con un decorator genérico sobre `ICommandHandler<TCommand, TResult>`, sin necesitar resolución dinámica.
- La inyección directa del handler concreto en cada endpoint es más legible en una revisión de código: se ve a simple vista, sin rastrear resoluciones en tiempo de ejecución, qué caso de uso invoca cada endpoint.
- Evitar la dependencia (o la reflexión de un dispatcher casero) reduce la superficie de conceptos a justificar, lo cual es preferible en un proyecto cuyo objetivo es demostrar dominio de los fundamentos (separación de responsabilidades, SOLID, CQRS) antes que el uso de una librería concreta.

## Propagación de `CancellationToken`

Todos los métodos de la cadena —desde el delegado del endpoint hasta la llamada final a `SaveChangesAsync` o a un cliente HTTP externo— reciben y propagan explícitamente el `CancellationToken` inyectado por el framework en el endpoint. Esto garantiza que si el cliente cancela la petición (cierra la conexión, hace timeout), el trabajo en curso —incluidas operaciones potencialmente costosas como la firma digital o el envío a la Hacienda Foral— se aborta de forma limpia en lugar de continuar ejecutándose sin que nadie espere el resultado. La única excepción deliberada es en tramos de la operación que, una vez iniciados, no deben interrumpirse para no dejar el sistema en un estado inconsistente (por ejemplo, un envío ya confirmado a la pasarela de la Hacienda Foral).

## Resumen de la decisión

|Aspecto|Decisión|Motivo|
|---|---|---|
|Separación de lecturas/escrituras|CQRS mediante `ICommand`/`IQuery`|Casos de uso claros en un dominio con reglas de negocio complejas|
|Estructura de cada caso de uso|Interfaces genéricas `ICommand<TResult>` / `ICommandHandler<TCommand, TResult>`|Aislamiento de dependencias, testabilidad, contratos explícitos|
|Resolución del handler|Inyección directa en el endpoint|Legibilidad, sin coste de reflexión, número de casos de uso reducido|
|Comportamiento transversal (validación, etc.)|Decorator genérico sobre `ICommandHandler`|Cubre la necesidad real sin necesitar dispatcher central|
|Patrón Mediador / MediatR|Descartado|Complejidad no justificada por el tamaño y alcance del proyecto|