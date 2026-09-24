### RNF-01 - Rendimiento - Prioridad: Alta
El sistema debe poder:
- Procesar una factura con OCR en menos de 5 segundos en condiciones normales.

### RNF-02 - Escalabilidad - Prioridad: Alta
La arquitectura debe permitir añadir módulos nuevos al sistema sin modificar o reestructurar la estructura base del sistema.

### RNF-03 - Disponibilidad - Prioridad: Media
El sistema debe estar disponible el 99% del tiempo.

### RNF-04 - Seguridad - Prioridad: Muy Alta
Todas las comunicaciones se deben realizar a través de HTTPS y las contraseñas deben almacenarse cifradas (bcrypt o similar).

### RNF-05 - Autenticación y autorización - Prioridad: Alta
La aplicación debe implementar control de acceso basado en roles (ej.: admin, usuario estándar...) y policies.

### RNF-06 - Usabilidad - Prioridad: Baja
La interfaz de usuario debe ser intuitiva, con un flujo claro desde la carga de la factura hasta su confirmación.

### RNF-07 - Mantenibilidad - Prioridad: Alta
El código debe seguir principios SOLID y Clean Architecture para facilitar cambios y extensiones futuras.

### RNF-08 - Portabilidad - Prioridad: Media
La aplicación debe poder ejecutarse en entornos Windows o Linux mediante Docker.

### RNF-09 - Compatibilidad - Prioridad: Media
Compatible con los navegadores modernos (Chrome, Firefox, Safari, Edge) pero también con los antiguos.

### RNF-10 - Auditoría - Prioridad: Alta
El sistema debe registrar los eventos críticos (inicio de sesión, creación, modificación y confirmación de facturas)

### RNF-11 - Respaldo y recuperación - Prioridad: Alta
Se debe realizar backup periódico de la BD.

### RNF-12 - Interoperabilidad futura - Prioridad: Alta
La arquitectura debe permitir integrar servicios externos (Hacienda, TicketBai, APIs contables...)

### RNF-13 - Pruebas - Prioridad: Alta
El sistema debe tener pruebas unitarias automatizadas para las capas de dominio y aplicación.

### RNF-14 - Documentación técnica - Prioridad: Alta
Todo el código y los módulos deben estar documentados (XML Docs + README)