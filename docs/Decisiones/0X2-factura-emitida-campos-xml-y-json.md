La tabla de facturas contienen dos columnas importantes donde se guardan las serializaciones de las facturas correspondientes:
	
- **XML**: Se almacena el XML enviado a la hacienda correspondiente, solo se inserta cuando la factura ha sido emitida (único punto de entrada). O si se ha creado una anulación sobre una previamente emitida.
	- Tipo de campo PostgreSQL: TEXT
	
- **JSON**: Se almacena la factura a nivel de negocio (sin tener que ver con las haciendas) en formato JSON.