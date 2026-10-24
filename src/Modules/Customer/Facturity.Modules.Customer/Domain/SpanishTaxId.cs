using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record SpanishTaxId : ITaxId
{
    public string Valor { get; }
    public SpanishTaxIdSubtype Tipo { get; }
    public TaxIdType TipoIdentificacionFiscal { get; }
    public bool? IsLegalEntity => Tipo == SpanishTaxIdSubtype.Cif;

    protected SpanishTaxId() { }

    private SpanishTaxId(string valorNif, SpanishTaxIdSubtype tipoNif)
    {
        Valor = valorNif;
        Tipo = tipoNif;
        TipoIdentificacionFiscal = TaxIdType.TaxId;
    }

    public static Result<SpanishTaxId> Crear(string valorNif)
    {
        var esNifValido = TaxIdValidations.IsValidSpanishTaxId(valorNif, out string codigoError, out string descripcionError);
        if (!esNifValido)
            return Result.Fail<SpanishTaxId>(Error.Validation(codigoError, descripcionError));

        var tipoNifResult = SpanishTaxIdService.GetSubtype(valorNif);

        if (tipoNifResult.IsFailure)
            return Result.Fail<SpanishTaxId>(tipoNifResult.Errors);

        return Result.Ok(new SpanishTaxId(valorNif, tipoNifResult.Value));
    }

}