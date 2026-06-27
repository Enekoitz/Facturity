### RF-01 - Autenticación de usuario - Prioridad: Alta
El sistema debe permitir que los usuarios se registren e inicien sesión mediante credenciales.

### RF-02 - Gestión de usuario y roles - Prioridad: Media
Apartado para gestionar los usuario y roles del sistema. 

### RF-03 - Gestión de productos o productos - Prioridad: Alta
Gestionar (crear, editar, eliminar) los productos o servicios.

### RF-04 - Gestión de clientes y series - Prioridad: Muy Alta
Gestionar (crear, editar, eliminar) clientes, series de facturas asociadas a clientes y sus preferencias por defecto (IVA, IRPF, descuentos, Recargo de equivalencia...)

### RF-05 - Carga de facturas manuscritas (imágenes) - Prioridad: Muy Alta
Subir o cargar imágenes o archivos escaneados de facturas manuscritas. Tendrá que controlarse la compresión y los archivos subidos se deberán almacenar correctamente.

### RF-06 - Procesamiento OCR (IA) - Prioridad: Muy Alta
El sistema procesará las imágenes/archivos previamente cargadas y extraerá los datos relevantes (productos, importes, fechas, clientes...)

### RF-07 - Edición y validación de factura digitalizada - Prioridad: Muy Alta
Tras extraer los datos de la factura, el usuario podrá validar los datos, editarlos y confirmar la factura.

### RF-08 - Generación de la factura - Prioridad: Muy Alta
Tras la validación y confirmación, se guarda y genera la factura con los datos correspondientes. 
Se genera también una factura en formato PDF que se visualizará con un visor de PDF. Con el que se podrá imprimir, guardar, etc.

### RF-09 - Gestión y consulta de facturas - Prioridad: Muy Alta
Apartado del sistema para consultar todas las facturas existentes en cualquier estado.

### RF-10 - Visor de facturas en PDF - Prioridad: Media
El sistema debe incluir un visor embebido para poder visualizar las facturas sin necesidad de descargarla a cada rato.

### RF-11 - Creación manual de factura - Prioridad: Media
El sistema debe proporcionar la manera de crear una factura de forma manual, introduciendo directamente los datos: cliente, detalle de factura, IVA, Tipo de actividad, etc.

### RF-12 - Estadísticas básicas - Prioridad: Media
Mostrar métricas y estadísticas básicas: número de facturas, ventas totales por periodo, producto más vendido...

### RF-13 - Exportación de datos - Prioridad: Media
Permitir exportar facturas o listados en formato CSV, Excel...

### RF-14 - Configuración de empresa - Prioridad: Alta
Permite configurar los datos fiscales y de contacto de la empresa emisora

### RF-15 - Historial y auditoría - Prioridad: Media
Registrar acciones relevantes (ej.: creación, edición y confirmación de facturas)


Más adelante, en la **Fase 2**, añadirías requerimientos como:

- Envío automático a Hacienda (TicketBAI API).
    
- Validación de firma y QR TicketBAI.
    
- Reportes analíticos avanzados.
    
- Integración con contabilidad externa.