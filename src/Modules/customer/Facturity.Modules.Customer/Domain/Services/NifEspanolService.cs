using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public static class NifEspanolService
{
    public static Result<TiposNifEspanol> ObtenerTipo(string valorIdentificacion)
    {
        if (IdentificacionFiscalValidaciones.EsDni(valorIdentificacion))
            return Result.Ok(TiposNifEspanol.Dni);

        else if (IdentificacionFiscalValidaciones.EsNie(valorIdentificacion))
            return Result.Ok(TiposNifEspanol.Nie);

        else if (IdentificacionFiscalValidaciones.EsCif(valorIdentificacion))
            return Result.Ok(TiposNifEspanol.Cif);

        return Result.Fail<TiposNifEspanol>(Error.NotFound("TipoNifInvalido", "El valor NIF no corresponde a ningún subtipo (DNI, NIE, CIF)"));
    }
}