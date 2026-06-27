---
status: accepted
date: 2026-06-26
---
# ADR-xxx VO único para Identificación Fiscal

## Contexto
TicketBai distingue NIF e IDOtro como campos excluyentes en el XML.
Necesito modelar la identificación fiscal de un cliente en el dominio.

## Opciones consideradas
- VO único con discriminación interna
- Dos VOs separados (IdentificacionNif e IdentificacionExtranjera)

## Decisión
VO único porque refleja que es un mismo concepto de dominio con dos 
representaciones, y simplifica el agregado Cliente.

## Consecuencias
- La lógica de qué campo usar al generar el XML queda encapsulada en el VO
- Si TicketBai añade más tipos en el futuro, el cambio está localizado

## Referencias
- [Identificacion Destinatario TicketBai](../integraciones/ticketbai/Identificacion Destinatario.md)