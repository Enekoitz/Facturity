# Requisitos

> **Propósito:** lo que el sistema debe cumplir, priorizado.
>
> **Preguntas guía:** ¿Qué debe hacer el sistema? ¿Con qué calidad (rendimiento, seguridad, accesibilidad)? ¿Qué pasa si falta este requisito?
>
> **Prioridad (MoSCoW):** **M**ust (imprescindible) · **S**hould (importante) · **C**ould (deseable) · **W**on't (no ahora).

## Funcionales

| ID     | Requisito                                                                                                                                                                                                                                     | Prioridad | Origen               | User Story |
| ------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------- | -------------------- | ---------- |
| RF-001 | _El sistema debe permitir que los usuarios se registren e inicien sesión mediante credenciales._                                                                                                                                              | M         | _Análisis funcional_ | pdte.      |
| RF-002 | _Los usuarios deben poder gestionar los clientes a los que asignar las facturas: crear, actualizar datos, eliminar del sistema._                                                                                                              | M         | _Análisis funcional_ | pdte.      |
| RF-003 | _Los usuarios deben poder gestionar series: crear y desactivar/activar._                                                                                                                                                                      | M         | _Análisis funcional_ | pdte.      |
| RF-004 | _Debe existir posibilidad de añadir preferencias concretas a los clientes (tipo de IVA, tipo de recargo de equivalencia, etc.)._                                                                                                              | C         | _Análisis funcional_ | pdte.      |
| RF-005 | _El usuario debe poder crear una factura manualmente con opción a dejar en un estado "en borrador" o enviarla directamente a la hacienda correspondiente._                                                                                    | M         | _Análisis funcional_ | pdte.      |
| RF-006 | _El usuario debe poder eliminar una factura que se encuentre en un estado previo a haber sido enviada a hacienda._                                                                                                                            | M         | _Análisis funcional_ | pdte.      |
| RF-007 | _Se debe poder crear una factura y enviar en el mismo momento a la hacienda correspondiente cumpliendo la validación de firma y generación del QR._                                                                                           | S         | _Análisis funcional_ | pdte.      |
| RF-008 | _Se debe poder anular una factura que haya sido enviada previamente a hacienda, pero no eliminarla._                                                                                                                                          | S         | _Análisis funcional_ | pdte.      |
| RF-009 | _El sistema debe controlar y establecer sin fallo (control de concurrencia, nunca usar un número antes empleado) el número de factura al enviar a hacienda._                                                                                  | S         | _Análisis funcional_ | pdte.      |
| RF-010 | _Se debe poder enviar varias facturas a hacienda en una única operación si están en un estado válido._                                                                                                                                        | S         | _Análisis funcional_ | pdte.      |
| RF-011 | _El sistema al ser multi-tenant debe asignar un tenant nuevo a cada usuario registrado._                                                                                                                                                      | M         | _Análisis funcional_ | pdte.      |
| RF-012 | _El sistema al ser multi-tenant debe dar opción a vincularse a un tenant (organización) existente vía código de invitación o algo similar._                                                                                                   | W         | _Análisis funcional_ | pdte.      |
| RF-013 | _El sistema debe obligar a pertenecer a un tenant válido, no de baja y con datos fiscales adecuados para que el usuario pueda generar facturas. Tras el registro del usuario es necesario introducir los datos del tenant si va a ser nuevo._ | M         | _Análisis funcional_ | pdte.      |
| RF-014 | _El usuario debe poder consultar el detalle de una factura concreta esté en el estado que esté._                                                                                                                                              | M         | _Análisis funcional_ | pdte.      |
| RF-015 | _El usuario debe poder listar facturas que coincidan con los filtros de búsqueda introducidos. Controlando paginación y máximo de elementos devueltos._                                                                                       | M         | _Análisis funcional_ | pdte.      |
| RF-016 | _El sistema debe permitir exportar a PDF la factura esté en el estado que esté._                                                                                                                                                              | W         | _Análisis funcional_ | pdte.      |
| RF-017 | _El sistema debe permitir cargar facturar manuscritas o en algún tipo de documento digital, escanear mediante un módulo OCR y generar borrador/es de facturas._                                                                               | W         | _Análisis funcional_ | pdte.      |
| RF-018 | _El sistema debe implementar un panel de estadísticas básicas configurables (diagramas, informes, métricas, estadísticas de negocio, etc.)._                                                                                                  | W         | _Análisis funcional_ | pdte.      |
| RF-019 | _El sistema debe estar correctamente construido para auditar las operaciones más relevantes y gestionar correctamente históricos, registros de acceso obligatorios, privacidad, etc._                                                         | W         | _Análisis funcional_ | pdte.      |
| RF-020 | _Debe implementarse un control de acceso basado en roles._                                                                                                                                                                                    | S         | _Análisis funcional_ | pdte.      |
| RF-021 | _El sistema debe poder procesa múltiples facturas simultáneamente mediante archivos estructurados (como CSV o Excel) aunque no enviar a hacienda sin revisión previa._                                                                        | C         | _Análisis funcional_ | pdte.      |
| RF-022 | _El sistema debe poder procesa múltiples facturas simultáneamente mediante archivos estructurados (como CSV o Excel) aunque no enviar a hacienda sin revisión previa._                                                                        | C         | _Análisis funcional_ | pdte.      |
| RF-023 | _Debe existir posibilidad de añadir preferencias concretas para las facturas._                                                                                                                                                                | C         | _Análisis funcional_ | pdte.      |

## No funcionales

| ID      | Categoría               | Requisito medible                                                                                                                                                | Prioridad |
| ------- | ----------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------- |
| RNF-001 | Rendimiento             | _Respuesta < 500 ms en el 95 % de las peticiones._                                                                                                               | S         |
| RNF-002 | Seguridad               | _Las comunicaciones deben realizarse a través de HTTPS._                                                                                                         | M         |
| RNF-003 | Seguridad               | _No puede filtrarse información o datos entre tenants._                                                                                                          | M         |
| RNF-004 | Escalabilidad           | La arquitectura del sistema debe posibilitar añadir módulos nuevos al sistema sin modificar o reestructurar la estructura base del sistema.                      | S         |
| RNF-005 | Disponibilidad          | _El sistema debe estar disponible el 99% del tiempo._                                                                                                            | S         |
| RNF-006 | Portabilidad            | _El sistema debe poder ejecutarse en entornos Windows o Linux mediante Docker._                                                                                  | S         |
| RNF-008 | Auditoría               | _El sistema debe registrar los eventos críticos (inicio de sesión, creación, modificación y confirmación de facturas)_                                           | M         |
| RNF-009 | Respaldo y recuperación | _Backups periódicas de la base de datos de producción._                                                                                                          | M         |
| RNF-010 | Interoperabilidad       | _La arquitectura debe permitir integrar servicios externos (Hacienda, TicketBai, APIs contables...)_                                                             |           |
| RNF-011 | Pruebas                 | _El sistema debe tener una buena cobertura de pruebas que garanticen las partes críticas y más importantes: algoritmos complejos, lógica de negocio, etc.._      |           |
| RNF-012 | Documentación           | _La documentación del sistema debe de estar actualizada y cubrir todo aquello que no pueda ser explicado directamente viendo el sistema, o que sea muy complejo_ |           |

## Reglas de negocio
_Reglas que el sistema debe respetar. Detalle de entidades en el [modelo de dominio](invoicing.md)._

(Pendiente definir - Prioridad baja porque lo importante está directamente en el modelo de dominio)

- RN-001: ---

---
_Última revisión: 2026-10-04_
