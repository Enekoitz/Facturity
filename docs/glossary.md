# Glosario

> **Propósito:** lenguaje común o ubicuo (DDD). Cada término del negocio con una única definición. Es la base del [modelo de dominio](invoicing.md) y del nombre de las clases en el código.
>
> **Preguntas guía:** ¿Hay palabras que distintas personas usan con significados distintos? ¿Qué término usa el negocio y cuál el código?

Grouped by bounded context / module:
## Customer 

| Término  | Definición                                                         | Evitar (sinónimos ambiguos en el sistema) | Nombre en código                     | TicketBai (Diputación Foral de Bizkaia) |
| -------- | ------------------------------------------------------------------ | ----------------------------------------- | ------------------------------------ | --------------------------------------- |
| Customer | Persona que compra bienes o servicios luego plasmado en la factura | usuario, cuenta, destinatario             | `Cliente`<br>*Pdt. cambia a Customer | Destinatario                            |
|          |                                                                    |                                           |                                      |                                         |

## Invoicing 

| Término | Definición | Evitar (sinónimos ambiguos en el sistema) | Nombre en código                     | TicketBai (Diputación Foral de Bizkaia) |
| ------- | ---------- | ----------------------------------------- | ------------------------------------ | --------------------------------------- |
| Invoice | Factura    | usuario, cuenta, destinatario             | `Invoice`<br>*Pdt. cambia a Customer | Factura                                 |
|         |            |                                           |                                      |                                         |

---
_Última revisión: 2026-10-10_
