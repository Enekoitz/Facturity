using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public class NifEspañol : IIdentificacionFiscal
{
    public string Valor { get; }
    public TiposNifEspañol Tipo { get; }
    public TiposIdentificacionFiscal TipoIdentificacionFiscal { get; }
    public bool? EsPersonaJuridica => Tipo == TiposNifEspañol.Cif;

    protected NifEspañol() { }

    private NifEspañol(string valorNif, TiposNifEspañol tipoNif)
    {
        Valor = valorNif;
        Tipo = tipoNif;
        TipoIdentificacionFiscal = TiposIdentificacionFiscal.NifEspañol;
    }

    public static Result<NifEspañol> Crear(string valorNif)
    {
        bool esNifValido = IdentificacionFiscalValidaciones.EsNifValido(valorNif, out string codigoError, out string descripcionError);
        if (!esNifValido)
        {
            return Result.Fail<NifEspañol>(Error.Validation(codigoError, descripcionError));
        }

        TiposNifEspañol tipoNif = NifEspañolService.ObtenerTipo(valorNif);

        return Result.Ok(new NifEspañol(valorNif, tipoNif));
    }

}