
### 1. Creación de facturas.

Dos tipos:

- Manual: se crea la factura manualmente introduciendo cada dato uno a uno.
    
- Automático+ revisión: Se carga la imagen o imagenes de las facturas y el módulo OCR la procesa, extrae los datos y los coloca en un panel tipo formulario con posibilidad de editar y validar.

Nota: La factura una vez creada o revisada debe confirmarse para que se guarde correctamente
#### 1.1. Estados:
- En borrador
    
- Sin confirmar
    
- Confirmada
### 2. Gestión de datos colaterales de la factura:

Se gestionan los distintos tipos de elementos relacionados con la factura. Dándole cierta identidad a cada cliente, producto, información fiscal, etc. No solo texto plano.

#### 2.1. Clientes

Permite crear, editar o eliminar clientes por defecto para agilizar la selección del cliente o automatizarlo meidante el motor OCR y seleccionarlo tras procesar el manuscrito.

#### 2.2. Productos/Servicios

Permite categorizar, crear, editar y eliminar productos o servicios con el que poder almacenar cada dato de la factura. Muy útil para después obtener información relevante y generar dashboards.

#### 2.3. Series

Gestionar series de facturas. Permiten etiquetar las facturas por el criterio que el negocio decida (ej.: una serie por usuario, etc.), llevando una enumeración por cada una de las facturas.

#### 2.4. Usuarios

Gestión de usuarios del sistema. Crear, modificar, asignar roles y accesos restringidos al sistema. Asociamos el usuario a un correo electrónico, para fases posteriores se dejan abiertas posibilidades de comunicación, pensar en abstracción y escalabilidad de diferentes métodos de comunicación.

#### 2.5. Roles

Gestionar y asignar roles o tipos de perfiles con los que dejar la posibilidad de restringir o dar acceso a cada funcionalidad.

### 3. Consulta y gestión de facturas existentes
#### 3.1. Consulta

Panel con múltiples filtros dinámicos con los que realizar consultas de facturas existentes.

Nota: sería interesante integrar la posibilidad de implementar dashboards en este mismo panel, que se cargue con la información proporcionada por las facturas que corresponden a lo filtrado.

#### 3.2. Gestión

Las facturas deben tener operaciones bien definidas y claras.

- **Anular**: se borra del sistema. Una confirmada o se puede modificar

- **Confirmar**: una factura en estado sin confirmar debe poder confirmarse.

- **Editar**: una factura en estado sin confirmar se puede confirmar.

- **Visualización**: Visor PDF con el que poder visualizar cada factura. ¿Generada o cargada desde el sistema de ficheros?