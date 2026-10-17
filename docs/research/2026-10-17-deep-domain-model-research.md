
This research file's main goal is to define system's first version domain defining what is out of scope and what isn't.

> Source: https://www.batuz.eus/fitxategiak/batuz/normativa/2020%2009%2011%20ORDEN%20FORAL%201482-2020,%20de%209%20de%20septiembre.pdf?hash=d8b23fcbb5796f4e6517391328d796e5
# TBAI scheme

- Cabecera > **IDVersionTBAI**. No hay que modelarlo, es una propiedad puramente técnica de ticketbai, sistema externo. Solo debemos tenerlo en cuenta durante el mapeo del esquema XSD que debe recibir la hacienda de la diputación de Bizkaia.

- **Sujetos**:
	- **Emisor**: Al ser ticketBai, solo se requiere NIF + NombreApellidos o RazonSocial. En nuestro caso esta entidad va ligada al TenantId. De hecho la idea es que la entidad Tenant recoja esos datos. No va a existir Emisor como entidad directa.
	- **Destinatarios**: En nuestro sistema es Customer (Cliente).
	
	- VariosDestinatarios: S/N. (Permite que haya hasta 100 destinatarios). ==OUT OF SCOPE V1==
	- EmitidaPorTercerosODestinatario: S/N ==OUT OF SCOPE V1==
	
- **Factura**:
		
	- **Cabecera**:
		- **Serie de factura**
		- **Número de factura**
		- **Fecha de expedición** (en el dominio del sistema fecha y hora junta como DateTimeOffset)
		- **Hora de expedición** (en el dominio del sistema fecha y hora junta como DateTimeOffset)
		- **Factura simplificada** 
		
		- Factura emitida en sustitución de una factura simplificada ==OUT OF SCOPE V1==
		- Factura rectificativa ==OUT OF SCOPE V1==
		- Factura rectificativa sustituida ==OUT OF SCOPE V1==
		
	- **Datos**:
		- **Fecha de operación**
		- **Descripción**
		- **Importe total**
		- **Claves** (Clave que identificará el tipo de régimen del IVA o una operación con trascendencia tributaria)
		
		- Retencion soportada ==OUT OF SCOPE V1==
		- Base imponible a coste ==OUT OF SCOPE V1==
		
	- **TipoDesglose**:
		- **DesgloseFactura** (contraparte nacional o no existe contraparte)
			- **Sujeta**
				- **NoExenta**
					- DetalleNoExenta (1 a 2):
						- TipoNoExenta
						- DesgloseIVA
							- DetalleIVA (1 a 6, una agrupación de datos por tipo)
								- Base imponible
								- TipoImpositivo
								- CuotaImpuesto
								
								- TipoRecargoEquivalencia ==OUT OF SCOPE V1==
								- CuotaRecargoEquivalencia ==OUT OF SCOPE V1==
								- OperacionEnRecargoDeEquivalencia -ORegimenSimplificado ==OUT OF SCOPE V1==


				- Exenta ==OUT OF SCOPE V1== 
					- DetalleExenta (1 a 7, 1 agrupación de datos por causa de exención). 
						- CausaExencion
						- BaseImponible
									
			- NoSujeta ==OUT OF SCOPE V1==
				- DetalleNoSujeta (1 a 2)
					- Causa
					- Importe

		- DesgloseTipoOperacion (contraparte no nacional) ==OUT OF SCOPE V1==






