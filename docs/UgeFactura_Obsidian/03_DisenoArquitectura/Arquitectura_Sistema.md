La aplicación sigue una arquitectura web modular basada en servicios REST, permitiendo independencia entre la capa de presentación (frontend), la capa lógica (backend) y la capa de datos (base de datos). Esto facilita la escalabilidad, mantenibilidad, testabilidad y futuras integraciones.

### Diagramas de arquitectura


### Componentes principales

| Módulo        | Descripción                         | Tecnología         | Responsabilidad                                               |
| ------------- | ----------------------------------- | ------------------ | ------------------------------------------------------------- |
| Facturación   | Generación y validación de facturas | ASP.NET / C#       | Encargado de crear facturas y comunicarlas con las haciendas. |
| OCR           | Captura de datos desde imágenes     | Python / .NET + IA | Procesar imágenes y extraer datos estructurados               |
| Base de datos | Persistencia de información         | PostgreSQL         | Almacena series, facturas, productos, clientes, etc.          |
| Frontend      | Interfaz web                        | Angular            | Interacción del usuario y visualización de datos              |
