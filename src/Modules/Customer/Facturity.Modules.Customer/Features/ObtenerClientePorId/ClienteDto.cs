using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record ClienteDto(
    string Alias,
    string IdentificacionFiscalValor,
    TaxIdType IdentificacionFiscalTipo,
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
    internal static ClienteDto ToDto(this Domain.Customer customer)
    {
        (string identificacionFiscalValor, 
            TaxIdType identificacionFiscalTipo, 
            string identificacionFiscalCodigoPais) = customer.TaxId.GetCommonValues();
        
        return new ClienteDto(
            customer.Alias,
            identificacionFiscalValor,
            identificacionFiscalTipo,
            identificacionFiscalTipo == TaxIdType.TaxId 
                ? null 
                : identificacionFiscalCodigoPais,
            customer.RazonSocial,
            customer.Nombre,
            customer.PrimerApellido,
            customer.SegundoApellido,
            customer.Domicilio?.Street,
            customer.Domicilio?.PostalCode?.Valor,
            customer.Domicilio?.City,
            customer.Domicilio?.State,
            customer.Domicilio?.Region,
            customer.Domicilio?.Country);
    }
}