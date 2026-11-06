Esta nota de research se crea por la necesidad de tener que remodelar el dominio de facturación al no haber sido modelado correctamente en el primer desarrollo.

Al configurar el modelo de datos con EF Core (Fluent API) hemos ido detectando muchas complicaciones que en su mayoría son evitables simplificando el modelo de dominio de facturación.

# ¿Por qué ese primer modelo no es válido?

En su día, durante la investigación y profundización del dominio nos fijamos demasiado en la composición de al estructura del XSD de TicketBai (la estructura de datos que requiere la DFB).

Esto fue una mala práctica. La estructura del XSD debe darnos información de lo que se requiere, es opcional, tipos de datos, catálogos, etc. Pero la estructura es importante en el momento en el que "traducimos" nuestra factura de nuestro modelo a la estructura de envío a Hacienda. Y eso pertenece a la Anticorruption Layer, no al dominio puro nuestro.

Específicamente fallamos en lo siguiente:

- Definir dos VOs InvoiceHeader e InvoiceData simulando los bloques que necesita el XSD. Puede ser interesante a primera vista pero realmente complica mucho todo dado que internamente tienen otros VOs que incluso son más importantes y, además, acabarán siendo entidades y no VOs. Realmente no tiene sentido porque son VOs artificiales que solo sirven para organizar código sin ser necesario. La realidad es que duplica archivos, configuraciones, mantenimiento. 
	
	- **Aprendizaje**: No forzar VOs ni dominio rico sacrificando pragmatismo. Los VOs deben ser elementos que simplifiquen y tengan responsabilidad acotada, no grupos que sirvan para estructurar. Tampoco sacar conclusiones directas de cómo debe ser un dominio desde un sistema tercero. Lo más adecuado suele ser tenerlo en cuenta pero que haya un anticorruption layer.
	
- Los VOs internos InvoiceSeries de InvoiceHeader y Lines (lista InvoiceLine) de InvioceData fueron definidos como VOs, pero claramente son entidades. Y los VOs padres no pueden tener hijos entidades, rompe DDD al completo. Los VOs no tienen identidad como tal, y no pueden tener hijos con identidad.


# Solución
Se remodela el dominio de Invoicing asegurando que las siguientes reglas, invariantes, comportamientos se cumplen sí o sí con pragmatismo y facilitando la posterior integración de la persistencia y demás. 
Estos cambios deben verse reflejados en el archivo del modelo de dominio de invoicing y luego en toda la solución con la implementación:

- InvoiceData e InvoiceHeader desaparecen del dominio. Todos sus miembros, validaciones, comportamiento, etc. pasan y se adaptan directamente a Invoice.
- Se crea un VO concreto IssuanceInfo que contiene: Number e IssuedAtUtc. En invoice será nullable. Esto es importante porque esos datos solo se llenan y obligatoriamente cuando la factura se emita. Aquí si tiene sentido que sea VO, algo acotado y con sentido de dominio.

## Resumen

```text
Invoice (Aggregate root)
	- Id
	- TenantId
	- CustomerId
	- SeriesId (+ navegation InvoiceSeries)
	- Status
	- IsSimplified
	- TransactionAtUtc
	- IssuanceInfo (VO)
		  - Number
		  - IssuedAtUtc
	- Description
	- Lines (InvoiceLine list)
	- TaxBreakdown
	- Currency
```

```text
InvoiceSeries (Entity)
	- Id
	- TenantId
	- SeriesId
	- Name
	- Description
```

```text
InvoiceLine (Entity)
	- Id
	- TenantId
	- InvoiceId
	- Other related data
```
# Pasos a seguir

1. Desarrollar, detallar y oficializar este documento bruto en la **documentación** del proyecto.

2. Dejar **registro en un ADR** de las decisiones de todo lo comentado, que quede reflejado y ampliar dicha decisión a todo el proyecto. Que quede constancia que desde el cambio actual se trabaja con esa decisión: dominio pragmático y siempre debe tener un sentido de dominio, no organizativo en cuanto a estructurar datos o sobreingenierizar. También siendo pragmáticos a la hora de tener en cuenta la infraestructura y el modelo de datos. El dominio está desacoplado completamente de EF Core y cualquier proveedor de BD, pero debe estar influido por el modelo de datos que queremos construir.

3. Implementar el **domino en código**: sacar rama de main, aplicar cambios en el dominio.

4. Adaptar todos los **tests unitarios e integración** relacionados: revisar qué estamos cubriendo.

5. Añadir persistencia con EF Core:
	1. Configuración de entidades: Invoice, InvoiceSeries, InvoiceLines