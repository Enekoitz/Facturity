namespace UgeFactura.Modules.Clientes.Features;

internal record ObtenerClientesCommand(string Alias, string IdentificacionTipoCodigo, string IdentificacionValor, string? IdentificacionPais,
                                    string? RazonSocial, string? Nombre, string? PrimerApellido, string? SegundoApellido, string? Direccion,
                                    string? CodigoPostal, string? Municipio, string? Provincia, string? Region, string? Pais);
