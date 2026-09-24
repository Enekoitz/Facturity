El número de serie de la factura se diseña como nullable porque dependiendo del estado de la factura no debemos asignarle el número de serie real disponible. 

Este número solo se asigna a la factura cuando, tras ser validada en el mismo instante, se emite la factura (firma + hash de al factura anterior) y se genera el XML de envío a Hacienda:

>"asignación atómica en el momento de emisión"