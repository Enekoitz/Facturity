# Runbook de facturación: IVA, tipos de factura y TicketBAI

Documento de referencia para modelar el dominio de facturación. **Estado: borrador pendiente de validación.** Está escrito de memoria y con criterio general; los importes, plazos, claves y nombres de campos deben contrastarse con la fuente oficial (Reglamento de Facturación, Ley del IVA o norma foral del territorio, y especificaciones técnicas de TicketBAI). Donde hay duda real aparece marcado con **[VERIFICAR]**.

## 1. Alcance

- **Para qué sirve:** vocabulario común y reglas de negocio para el módulo de facturación.
- **Alcance v1 del proyecto:** factura emitida a cliente nacional, operación sujeta y no exenta, completa o simplificada, con uno o varios tipos de IVA.
- **Fuera de v1:** exentas, no sujetas, recargo de equivalencia, inversión del sujeto pasivo, regímenes especiales, clientes extranjeros, rectificativas, huella, firma y envío.
- El resto del documento describe todo el panorama para que el modelo no se cierre puertas.

## 2. Marco normativo (orientativo)

| Ámbito              | Norma                                                     | Para qué se usa                                                       |
| ------------------- | --------------------------------------------------------- | --------------------------------------------------------------------- |
| Estatal             | Ley del IVA (Ley 37/1992)                                 | Qué está sujeto, exento, tipos, rectificación                         |
| Estatal             | Reglamento de Facturación (RD 1619/2012)                  | Quién debe facturar, contenido, plazos, simplificadas, rectificativas |
| Territorios forales | Normas forales del IVA de Bizkaia, Gipuzkoa y Álava       | Equivalentes a la ley estatal, con matices propios                    |
| Territorios forales | Decretos forales de TicketBAI y especificaciones técnicas | Obligación del software, esquema XML, firma, huella, envío            |

Nota: en el País Vasco el IVA lo gestionan las haciendas forales, con normativa muy alineada con la estatal. Los matices (plazos, regímenes, claves) conviene comprobarlos en la norma foral de tu territorio. **[VERIFICAR]**

## 3. Conceptos básicos

| Concepto           | Código sugerido   | Definición                                                         |
| ------------------ | ----------------- | ------------------------------------------------------------------ |
| Base imponible     | `TaxBase`         | Importe sobre el que se calcula el impuesto, antes de IVA          |
| Tipo impositivo    | `TaxRate`         | Porcentaje de IVA aplicable                                        |
| Cuota              | `TaxAmount`       | Base por tipo, en euros                                            |
| Total              | `TotalAmount`     | Base más cuota (menos retenciones, si las hay, a efectos de cobro) |
| Emisor / expedidor | `Issuer` (tenant) | Quien emite la factura                                             |
| Destinatario       | `Customer`        | Quien recibe la factura                                            |
| Serie              | `InvoiceSeries`   | Agrupación numerada de facturas                                    |
| Número             | `InvoiceNumber`   | Posición correlativa dentro de la serie                            |
| Desglose           | `BreakdownLine`   | Resumen fiscal por tipo impositivo                                 |
| Línea de detalle   | `InvoiceLine`     | Cada concepto facturado                                            |

Fórmulas de una línea:

- Base = unidades por precio unitario, redondeada a 2 decimales.
- Cuota = base por tipo / 100, redondeada a 2 decimales.
- Total = base más cuota.

Ejemplo: base 100,00 al 21 % da cuota 21,00 y total 121,00.

## 4. Tipos de factura

### 4.1 Factura completa (ordinaria)

Lleva la identificación completa del emisor y del destinatario. Es la obligatoria cuando el destinatario es empresario o profesional y la necesita para deducir el IVA, o cuando el cliente la solicita con fines fiscales.

Contenido habitual exigido **[VERIFICAR]**:

- Número y, en su caso, serie (correlativo dentro de la serie).
- Fecha de expedición.
- Fecha de la operación, si es distinta de la de expedición.
- Nombre o razón social, NIF y domicilio del emisor y del destinatario.
- Descripción de las operaciones, con los datos necesarios para determinar la base.
- Tipo o tipos impositivos aplicados.
- Cuota tributaria, por separado.
- Base imponible, desglosada por tipo impositivo.
- Si la operación está exenta o es de inversión del sujeto pasivo, referencia a la norma que lo ampara o mención equivalente.

### 4.2 Factura simplificada (ticket)

Se emite sin identificar al destinatario. Es el caso de venta minorista y comercio al por menor.

- Se puede emitir, entre otros casos, cuando el importe no supera **400 € IVA incluido**, o **3.000 € IVA incluido** en ciertas actividades (por ejemplo ventas al por menor, transporte de personas, hostelería) **[VERIFICAR umbrales y casos]**.
- También cuando se trata de una factura rectificativa en ciertos supuestos.
- No lleva obligatoriamente destinatario ni cuota por separado: puede indicar el tipo y la cuota incluida en el precio **[VERIFICAR]**.
- Si el cliente es empresario y necesita deducir, puede pedir factura completa, y el emisor debe expedirla.
- En TicketBAI se distingue en el esquema con un indicador de factura simplificada, y el bloque de destinatarios solo es obligatorio en la completa.

Regla de modelo propuesta para v1: simplificada igual a sin destinatario; completa igual a destinatario obligatorio al emitir. En la normativa una simplificada puede incluir datos del cliente si este los aporta, por lo que es una restricción de tu modelo y no una equivalencia legal.

### 4.3 Factura rectificativa

Corrige una factura ya emitida. **Nunca se edita la original**: se emite otra que la referencia.

Causas habituales **[VERIFICAR artículo exacto]**:

- Error en datos de la factura (base, cuota, tipo, identificación).
- Devolución de mercancías o envases.
- Descuentos o bonificaciones posteriores a la emisión.
- Resolución total o parcial de la operación.
- Impago, en los supuestos que la ley contempla (créditos incobrables, concurso).

Modalidades:

- **Por sustitución:** sustituye íntegramente a la original. Lleva los datos corregidos y, además, el importe de la rectificación.
- **Por diferencias:** solo recoge la diferencia (positiva o negativa) respecto a la original.

Qué debe indicar:

- Serie y número propios, en serie rectificativa si se usa.
- Referencia a la factura o facturas rectificadas (serie, número, fecha).
- Motivo o causa de la rectificación (en TicketBAI, mediante un código de causa).
- Importes rectificados, que pueden ser negativos.

Plazo para emitirla: el que marca la norma para cada causa (con carácter general, dentro del plazo de prescripción, 4 años). **[VERIFICAR]**

Impacto en el modelo: `Invoice` pasa a tener un tipo (ordinaria o rectificativa), una referencia a la rectificada, un motivo y la modalidad. Los importes pueden ser negativos, y `Money` hoy no los admite. Es la razón principal de dejarla en v2.

### 4.4 Otros tipos de documento (para saber que existen)

|   |   |   |
|---|---|---|
|Tipo|Idea|Estado en el proyecto|
|Factura recapitulativa|Agrupa varias operaciones del mismo cliente en un periodo (no superior a un mes natural)|Fuera|
|Factura proforma / presupuesto|No es factura, no tiene efectos fiscales|Fuera|
|Factura de sustitución de simplificada|Completa que sustituye a una simplificada previa|Fuera|
|Autofactura|La emite el destinatario en nombre del proveedor (casos tasados)|Fuera|
|Factura electrónica estructurada (por ejemplo B2B)|Obligaciones adicionales de formato y envío|Fuera, fijarse en la normativa que se vaya aplicando **[VERIFICAR]**|

## 5. Clasificación fiscal de la operación

Son dos preguntas distintas.

**Pregunta 1: ¿entra la operación en el ámbito del IVA?**

- **Sujeta:** dentro del ámbito del impuesto. Es el caso mayoritario.
- **No sujeta:** fuera del ámbito. No es un perdón, es que la ley dice que ahí no aplica. Ejemplos: operaciones no empresariales, ciertas reglas de localización (servicios que se consideran prestados fuera del territorio de aplicación del impuesto), entregas de muestras sin contraprestación en ciertos casos **[VERIFICAR]**.

**Pregunta 2: si es sujeta, ¿se paga IVA?**

- **No exenta:** sujeta y se paga. Lleva tipo, cuota y total.
- **Exenta:** sujeta, pero la ley libera del pago por una causa concreta (sanidad, enseñanza reglada, ciertos servicios financieros y de seguros, exportaciones y entregas intracomunitarias, entre otras).

```
Operación
├── No sujeta      (fuera del IVA, con causa)
└── Sujeta
    ├── Exenta     (dentro del IVA, sin pagarlo, con causa)
    └── No exenta  (se paga: tipo + cuota)
        ├── Sin inversión del sujeto pasivo (normal)
        └── Con inversión del sujeto pasivo
```

|   |   |   |   |
|---|---|---|---|
|Caso|Tipo y cuota|Qué lleva en su lugar|Total|
|Sujeta no exenta|Sí|Tipo y cuota|Base más cuota|
|Sujeta exenta|No|Causa de exención|Igual a la base|
|No sujeta|No|Causa de no sujeción|Igual a la base|

Aviso importante: una operación al **0 %** es sujeta y no exenta con tipo cero, y fiscalmente no es lo mismo que una exenta. No uses `TaxRate = 0` para representar exenciones.

## 6. Tipos impositivos de IVA (España, orientativo)

|   |   |   |
|---|---|---|
|Tipo|Porcentaje|Aplicación típica|
|General|21 %|La mayoría de bienes y servicios|
|Reducido|10 %|Alimentos elaborados, transporte de viajeros, hostelería, vivienda nueva en ciertos casos, entre otros|
|Superreducido|4 %|Alimentos básicos, libros, medicamentos, entre otros|

Notas:

- Ha habido rebajas temporales de tipos sobre determinados alimentos y suministros, con vigencia limitada. Consulta la normativa vigente en la fecha de la factura. **[VERIFICAR]**
- El tipo aplicable lo determina la normativa vigente **en la fecha de devengo**, no la fecha de emisión.
- El tipo es un dato de la factura, nunca se recalcula.
- Se admiten decimales (recargo de equivalencia: 5,2 %, 1,4 %, 0,5 %), por lo que `TaxRate` debe ser `decimal`.
- Canarias, Ceuta y Melilla no aplican IVA, tienen otros impuestos (IGIC, IPSI). Fuera de alcance.

## 7. Regímenes y casos especiales

### 7.1 Recargo de equivalencia

Régimen para comerciantes minoristas. El proveedor repercute, además del IVA, un recargo que luego el minorista no declara por separado.

- Recargos habituales: 5,2 % (sobre 21 %), 1,4 % (sobre 10 %), 0,5 % (sobre 4 %); tabaco con otro recargo propio **[VERIFICAR]**.
- En la factura aparece como un concepto adicional por cada tipo, con su base, tipo de recargo y cuota de recargo.
- Cambia la forma del desglose: cada `BreakdownLine` puede llevar cuota de recargo.

### 7.2 Inversión del sujeto pasivo

El IVA lo liquida el destinatario, no el emisor. La factura se emite **sin cuota** y con una mención expresa (por ejemplo, referencia a la inversión del sujeto pasivo).

- Casos típicos: ciertas operaciones de construcción entre empresarios, entregas de chatarra, algunas operaciones inmobiliarias y con oro **[VERIFICAR lista]**.
- En TicketBAI se desglosa en un bloque distinto del de no exenta sin inversión, de ahí el "1 a 2 detalles" del esquema.

### 7.3 Operaciones con el extranjero

|   |   |   |
|---|---|---|
|Operación|Idea general|Efecto en factura|
|Exportación (fuera de la UE)|Exenta|Sin IVA, con causa de exención|
|Entrega intracomunitaria (B2B)|Exenta|Sin IVA, con NIF-IVA del cliente|
|Adquisición intracomunitaria|IVA lo autorrepercute el comprador|Inversión del sujeto pasivo|
|Servicios a empresarios de otro país|Normalmente no sujetos en España por localización|Sin IVA, con causa de no sujeción|
|Ventas a particulares de la UE|Reglas específicas (ventanilla única, umbrales)|Casos aparte|

Se identifica al cliente con NIF-IVA (VIES) u otros identificadores en lugar del NIF nacional. Fuera de v1.

### 7.4 Otros regímenes (solo mapa)

- **REBU (bienes usados, objetos de arte, antigüedades):** el IVA se calcula sobre el margen, y la factura no desglosa cuota.
- **Régimen especial de agencias de viajes.**
- **Criterio de caja:** el IVA se devenga al cobrar o pagar, con requisitos y mención en factura.
- **Régimen simplificado y agricultura, ganadería y pesca (REAGP):** compensaciones y reglas especiales.
- **Retenciones de IRPF en facturas de profesionales:** no son IVA, pero afectan al total a cobrar (por ejemplo, retención del 15 % u otros porcentajes). No forman parte de la base ni de la cuota.

Nada de esto entra en v1. Basta con documentarlo como "fuera de alcance, así se modelaría".

## 8. Numeración y series

- La numeración debe ser **correlativa dentro de cada serie**, sin huecos ni duplicados.
- Se pueden usar varias series (por ejemplo, ordinarias, rectificativas, simplificadas, por establecimiento).
- La numeración de **rectificativas** puede ir en serie propia, según la norma.
- Número y serie identifican de forma única la factura.
- En TicketBAI, el esquema limita la longitud de serie y número **[VERIFICAR límites, creo que 20 caracteres cada uno]**.

Decisiones tomadas en el proyecto:

- El número se asigna **al emitir**, dentro de la transacción, con `UPDATE ... RETURNING` sobre la fila de la serie. No se usan secuencias de base de datos, porque no son transaccionales y dejarían huecos.
- El borrador no tiene número.
- La cabecera guarda `SeriesId` (referencia) y, al emitir, un snapshot con `SeriesCode` y `Number`.

## 9. Fechas y plazos

|   |   |   |
|---|---|---|
|Concepto|Regla general|Nota|
|Fecha de expedición|Día en que se emite la factura|Obligatoria|
|Fecha de operación|Cuando se realiza la entrega o servicio|Solo si difiere|
|Plazo de expedición|Generalmente en el momento de la operación; para destinatarios empresarios, a más tardar el día 16 del mes siguiente al devengo **[VERIFICAR]**||
|Conservación|Al menos 4 años (plazo de prescripción), el software de TicketBAI exige conservar los datos conforme a la norma **[VERIFICAR]**||
|Rectificación|Dentro del plazo marcado por la causa||

En TicketBAI hay además un plazo para enviar el registro a la hacienda foral tras la emisión (dentro del que se admite el envío diferido). **[VERIFICAR plazo exacto según territorio]**

## 10. TicketBAI: panorama

TicketBAI es el sistema de control de facturación de las haciendas forales vascas. El software de facturación debe cumplir requisitos técnicos.

**Qué exige, a grandes rasgos [VERIFICAR]:**

- Generar por cada factura un registro XML con el esquema de la hacienda foral.
- **Encadenar** cada registro con el anterior mediante una huella (hash) que incluye datos de la factura anterior.
- **Firmar** el XML (firma electrónica XAdES).
- Incluir un **identificador TBAI** y un **código QR** en la factura.
- Enviar los registros a la hacienda foral (en Bizkaia, junto con el sistema Batuz y su libro registro LROE, dependiendo del caso).
- Conservar los ficheros.
- No permitir alterar ni borrar facturas ya emitidas (se anulan o rectifican).

**Estructura de primer nivel del documento [VERIFICAR nombres]:**

- `Cabecera`: versión del esquema.
- `Sujetos`: emisor y destinatarios.
- `Factura`: cabecera de factura (serie, número, fecha, hora, indicador de simplificada, sustitución o rectificativa), datos de factura (descripción, importe total, claves de régimen) y tipo de desglose.
- `HuellaTBAI`: software, encadenamiento con la factura anterior.
- `Signature`: firma.

**Desglose en el esquema:**

```
TipoDesglose
└── DesgloseFactura
    ├── Sujeta
    │   ├── Exenta       (detalles con causa de exención)
    │   └── NoExenta
    │       └── DetalleNoExenta (1..2)   tipo: con o sin inversión del sujeto pasivo
    │           └── DesgloseIVA
    │               └── DetalleIVA (1..6) base, tipo, cuota (y recargo si aplica)
    └── NoSujeta         (detalles con causa de no sujeción)
```

Las cardinalidades y nombres exactos hay que confirmarlos en el XSD de tu territorio. Notas relevantes:

- El esquema no pide unidades ni precio unitario por línea en todos los casos; esa información es tuya.
- Máximo de tipos distintos por detalle (creo que 6): restricción de la ACL, no del dominio.
- Hay **claves de régimen** (código que describe el régimen o tipo de operación) que debes elegir según el caso. **[VERIFICAR catálogo]**
- Los importes tienen formato numérico con un número fijo de decimales; el formateo es responsabilidad de la ACL.

Variantes por territorio: Bizkaia, Gipuzkoa y Álava tienen endpoints, plazos y detalles distintos. Un adaptador por territorio detrás del mismo puerto.

## 11. Reglas de cálculo y redondeo

- Todo importe y tipo es `decimal`. Nunca `float` ni `double`.
- Redondeo a 2 decimales con `MidpointRounding.AwayFromZero`, siempre vía `InvoiceRounding.Round(...)`.
- Cadena: base, luego cuota sobre la base ya redondeada, luego total como suma de dos valores ya redondeados.
- Precio unitario: hasta 6 decimales, con error si se supera (nunca redondear en silencio).
- Unidades: hasta 2 decimales en v1, con error si se supera.
- Redondeo por línea: la cuota de cada línea se redondea individualmente.

**Punto abierto [VERIFICAR contra el validador de tu hacienda foral]:** la cuota del desglose por tipo puede calcularse como suma de cuotas de línea ya redondeadas o como tipo aplicado a la base agrupada. Puede diferir en céntimos. Ejemplo: tres líneas de base 0,50 al 21 % dan 0,11 por línea (suma 0,33), pero 1,50 por 21 % da 0,32.

Casos de prueba clave: puntos medios exactos (0,125; 0,145; 0,105), varios tipos en la misma factura, líneas con distinto número de decimales.

## 12. Mapa al modelo de dominio

|   |   |   |
|---|---|---|
|Concepto fiscal|Representación|Notas|
|Factura|`Invoice` (agregado raíz)|Estados: `Draft`, `Issued`|
|Cabecera|`InvoiceHeader` (VO)|`SeriesId`, `SeriesCode`, `Number`, fecha, `IsSimplified`|
|Línea|`InvoiceLine` (VO, record)|Descripción, unidades, precio unitario, tipo, base, cuota, total|
|Desglose|`BreakdownLine`|Derivado por tipo impositivo|
|Importe|`Money`|2 decimales|
|Precio unitario|`UnitPrice`|Hasta 6 decimales|
|Tipo|`TaxRate`|Porcentaje, 0 a 100|
|Serie|`InvoiceSeries`|Entidad con contador|
|Destinatario|`CustomerId` más snapshot al emitir|Pendiente de modelar|

Reglas del agregado:

- Solo una factura en `Draft` puede editarse o emitirse.
- Una `Issued` es inmutable.
- Simplificada sin destinatario; completa con destinatario obligatorio al emitir.
- Mínimo una línea al emitir.
- El dominio no asigna número; lo asigna la infraestructura en la transacción.
- Máximo de tipos distintos: restricción de la ACL, que debe devolver error de negocio.

## 13. Qué no modelar como estado de la factura

Para evitar un `Status` con diez valores:

- Enviada, aceptada o rechazada por Hacienda: estado del **envío**, no de la factura.
- Pagada o vencida: estado de **cobros**.
- Anulada: se resuelve con una factura rectificativa o un registro de anulación de TicketBAI, no editando la original **[VERIFICAR mecanismo de anulación en tu territorio]**.

## 14. Hoja de ruta de alcance

|   |   |
|---|---|
|Versión|Contenido|
|v1|Cliente nacional, sujeta y no exenta, completa y simplificada, varios tipos, desglose|
|v1.5|Exenta y no sujeta con causas (`TaxTreatment` como unión)|
|v2|Rectificativas (importes negativos, referencia a la original), recargo de equivalencia, inversión del sujeto pasivo|
|v2|Huella, firma, envío real, snapshot de destinatario|
|Más adelante|Clientes extranjeros, regímenes especiales|

## 15. Lista de validación

Marca cada punto cuando lo hayas contrastado con la fuente oficial.

- Umbrales de factura simplificada (400 € y 3.000 €) y supuestos.
- Contenido obligatorio de factura completa.
- Plazo de expedición y plazo de envío a la hacienda foral de tu territorio.
- Tipos impositivos vigentes y fecha de devengo.
- Causas de rectificación y plazo; modalidades por sustitución y por diferencias.
- Lista de operaciones con inversión del sujeto pasivo.
- Recargos de equivalencia vigentes.
- Límites de longitud de serie y número en el XSD.
- Nombres y cardinalidades de `DetallesFactura`, `TipoDesglose`, `DetalleNoExenta`, `DetalleIVA`.
- Catálogo de claves de régimen y causas de exención y no sujeción.
- Método de cálculo de la cuota del desglose (suma de líneas o tipo sobre base agrupada) y tolerancia del validador.
- Mecanismo de anulación de TicketBAI en tu territorio.
- Obligación y formato de conservación.

## 16. Fuentes sugeridas

- Ley del IVA y Reglamento de Facturación (BOE).
- Norma foral del IVA de tu territorio.
- Página oficial de TicketBAI de tu hacienda foral: decreto, especificaciones técnicas, XSD, FAQ y ejemplos XML.
- Un asesor fiscal para los casos límite.

---
_Last review: 2026-10-17_
