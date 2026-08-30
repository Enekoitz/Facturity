using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record IdOtroExtranjero : IIdentificacionFiscal
{
    public string Valor { get; }
    public TiposIdentificacionFiscal Tipo { get; }
    public string CodigoPais { get; }
    public bool? EsPersonaJuridica { get; } = null;

    private IdOtroExtranjero() { }

    private IdOtroExtranjero(string valorIdOtroExtranjero, TiposIdentificacionFiscal tipoIdOtroExtranjero, string codigoPais)
    {
        Valor = valorIdOtroExtranjero;
        Tipo = tipoIdOtroExtranjero;
        CodigoPais = codigoPais;
    }

    public static Result<IdOtroExtranjero> Crear(string valorIdOtroExtranjero, TiposIdentificacionFiscal tipoIdOtroExtranjero, string codigoPais)
    {
        if (!IdentificacionFiscalValidaciones.EsIdOtroExtranjeroValido(codigoPais, tipoIdOtroExtranjero, valorIdOtroExtranjero, out string codigoError, out string descripcionError))
            return Result.Fail<IdOtroExtranjero>(Error.Validation(codigoError, descripcionError));

        return Result.Ok(new IdOtroExtranjero(valorIdOtroExtranjero, tipoIdOtroExtranjero, codigoPais));
    }
}