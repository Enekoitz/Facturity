using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record NifEspanol : IIdentificacionFiscal
{
    public string Valor { get; }
    public TiposNifEspanol Tipo { get; }
    public TiposIdentificacionFiscal TipoIdentificacionFiscal { get; }
    public bool? EsPersonaJuridica => Tipo == TiposNifEspanol.Cif;

    protected NifEspanol() { }

    private NifEspanol(string valorNif, TiposNifEspanol tipoNif)
    {
        Valor = valorNif;
        Tipo = tipoNif;
        TipoIdentificacionFiscal = TiposIdentificacionFiscal.Nif;
    }

    public static Result<NifEspanol> Crear(string valorNif)
    {
        var esNifValido = IdentificacionFiscalValidaciones.EsNifValido(valorNif, out string codigoError, out string descripcionError);
        if (!esNifValido)
            return Result.Fail<NifEspanol>(Error.Validation(codigoError, descripcionError));

        var tipoNifResult = NifEspanolService.ObtenerTipo(valorNif);

        if (tipoNifResult.IsFailure)
            return Result.Fail<NifEspanol>(tipoNifResult.Errors);

        return Result.Ok(new NifEspanol(valorNif, tipoNifResult.Value));
    }

}