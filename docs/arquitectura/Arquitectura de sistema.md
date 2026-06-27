### Arquitectura de sistema: **Monolito Modular**.

Utilizamos monolito modular como punto medio entre la complejidad de los microservicios y el desorden del monolito tradicional.
##### ¿Por qué?
El monolito modular permite dividir a nivel lógico el sistema en distintos módulos (`bounded contexts`) aislándolos en ensamblados y utilizando distintas técnicas y tecnologías para que se puedan comunicar entre ellos, dotando al sistema de una alta cohesión, imponiendo un encapsulamiento al detalle y minimizando el acoplamiento entre módulos. 

La comunicación inter-modular es asíncrona por defecto para mantener el desacoplamiento temporal

A nivel físico sigue siendo una única unidad de despliegue, ejecutándose como un único proceso. Lo que facilita enormemente los procesos de despliegue y reduce la carga de red e infraestructura propia de los microservicios.

Además, nos permite que cada uno de los módulos implemente su tipo de arquitectura de software, sin estar obligados a que toda la aplicación implemente una única arquitectura. 

>NOTA: Se define la arquitectura de software de cada módulo en la siguiente página: [[Arquitectura de software]]

Si en un futuro se decide refactorizar el proyecto a una arquitectura de microservicios, la transición desde un monolito modular es más natural y menos compleja. 

### Diagramas de arquitectura


### Componentes principales

| Módulo        | Descripción                         | Tecnología         | Responsabilidad                                               |
| ------------- | ----------------------------------- | ------------------ | ------------------------------------------------------------- |
| Facturación   | Generación y validación de facturas | ASP.NET / C#       | Encargado de crear facturas y comunicarlas con las haciendas. |
| OCR           | Captura de datos desde imágenes     | Python / .NET + IA | Procesar imágenes y extraer datos estructurados               |
| Base de datos | Persistencia de información         | PostgreSQL         | Almacena series, facturas, productos, clientes, etc.          |
| Frontend      | Interfaz web                        | Angular            | Interacción del usuario y visualización de datos              |
