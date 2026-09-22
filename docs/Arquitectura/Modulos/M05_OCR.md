Procesa la factura 

Flujo previsto:
1. El usuario sube una imagen (JPG, PNG, PDF escaneado)
	
2. El sistema procesa la imagen con OCR (modelo general o personalizado)
	
3. Se identifican campos clave: cliente, fecha, productos, importes, etc.
	
4. Se muestran una vista previa editable con los datos reconocidos
	
5. El usuario confirma la factura para su registro definitivo
	

Consideraciones:
- Se podrá usar un modelo preentrenado (Azure, Tesseract, etc.) inicialmente.
	
- En una fase posterior se podrá entrenar un modelo propio para escritura personalizada.
	
