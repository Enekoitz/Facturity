using System.Diagnostics;
using Facturity.Modules.Customer.Domain;
using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Features;

internal sealed record ListarClientesRequest(
    string? Alias, 
    string? IdentificacionTipoCodigo, 
    string? IdentificacionValor, 
    string? IdentificacionPais, 
    string? DenominacionFiscal,
    string? Direccion,
    bool? Activo,
    int Page = 1,
    int Size = 20);

internal static class ListarClientesRequestMapperExtension
{
    internal static Result<ListarClientesQuery> MapToQuery(this ListarClientesRequest request)
    {
        TaxIdType? codigoIdentificacionFiscal;
        if (string.IsNullOrWhiteSpace(request.IdentificacionTipoCodigo))
            codigoIdentificacionFiscal = null;
        else if (Enum.TryParse<TaxIdType>(request.IdentificacionTipoCodigo, ignoreCase: true, out var e)
                 && Enum.IsDefined(typeof(TaxIdType), e))
            codigoIdentificacionFiscal = e;
        else
            return Result.Fail<ListarClientesQuery>(CustomerErrors.TaxIdTypeInvalid);

        int page =  request.Page <= 0 ? 1 : request.Page;
        int size = request.Size switch
        {
            <= 0 => 10,
            > 100 => 100,
            _ => request.Size
        };
            
        
        var query = new ListarClientesQuery(
            request.Alias,
            codigoIdentificacionFiscal,
            request.IdentificacionValor,
            request.IdentificacionPais,
            request.DenominacionFiscal,
            request.Direccion,
            request.Activo,
            page,
            size);

        return Result.Ok(query);
    }
}