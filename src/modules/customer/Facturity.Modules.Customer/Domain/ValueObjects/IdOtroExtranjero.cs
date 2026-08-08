using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class IdOtroExtranjero : IIdentificacionFiscal
{
    public string Valor { get; }
    public TiposIdsOtrosExtranjeros Tipo { get; }
    public string CodigoPais { get; }
    public TiposIdentificacionFiscal TipoIdentificacionFiscal { get; }
    public bool? EsPersonaJuridica { get; } = null;

    protected IdOtroExtranjero() { }

    private IdOtroExtranjero(string valorIdOtroExtranjero, TiposIdsOtrosExtranjeros tipoIdOtroExtranjero, string codigoPais)
    {
        Valor = valorIdOtroExtranjero;
        Tipo = tipoIdOtroExtranjero;
        CodigoPais = codigoPais;
        TipoIdentificacionFiscal = TiposIdentificacionFiscal.IdOtroExtranjero;
    }

    public static Result<IdOtroExtranjero> Crear(string valorIdOtroExtranjero, TiposIdsOtrosExtranjeros tipoIdOtroExtranjero, string codigoPais)
    {

        bool esIdOtroExtranjeroValido = IdentificacionFiscalValidaciones.EsIdOtroExtranjeroValido(codigoPais, tipoIdOtroExtranjero, valorIdOtroExtranjero, out string codigoError, out string descripcionError);
        if (!esIdOtroExtranjeroValido)
        {
            return Result.Fail<IdOtroExtranjero>(Error.Validation(codigoError, descripcionError));
        }

        return Result.Ok(new IdOtroExtranjero(valorIdOtroExtranjero, tipoIdOtroExtranjero, codigoPais));
    }
}