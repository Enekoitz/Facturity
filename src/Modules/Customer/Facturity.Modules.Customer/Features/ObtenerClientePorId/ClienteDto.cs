using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record ClienteDto(
    string Alias,
    string IdentificacionFiscalValor,
    TiposIdentificacionFiscal IdentificacionFiscalTipo,
    string? IdentificacionFiscalCodigoPais,
    string? RazonSocial,
    string? Nombre,
    string? PrimerApellido,
    string? SegundoApellido,
    string? Direccion,
    string? CodigoPostal,
    string? Municipio,
    string? Provincia,
    string? Region,
    string? Pais) { }

internal static class ClienteDtoExtensions
{
    internal static ClienteDto ToDto(this Cliente cliente)
    {
        (string identificacionFiscalValor, 
            TiposIdentificacionFiscal identificacionFiscalTipo, 
            string identificacionFiscalCodigoPais) = cliente.IdentificacionFiscal.ObtenerValoresComunes();
        
        return new ClienteDto(
            cliente.Alias,
            identificacionFiscalValor,
            identificacionFiscalTipo,
            identificacionFiscalTipo == TiposIdentificacionFiscal.Nif 
                ? null 
                : identificacionFiscalCodigoPais,
            cliente.RazonSocial,
            cliente.Nombre,
            cliente.PrimerApellido,
            cliente.SegundoApellido,
            cliente.Domicilio?.Direccion,
            cliente.Domicilio?.CodigoPostal?.Valor,
            cliente.Domicilio?.Municipio,
            cliente.Domicilio?.Provincia,
            cliente.Domicilio?.Region,
            cliente.Domicilio?.Pais);
    }
}