using System.Diagnostics.CodeAnalysis;

namespace UgeFactura.Modules.DomainSharedKernel;

public static class Validar
{
    public static bool EsVacio([NotNullWhen(false)] string? valor)
    {
        return string.IsNullOrWhiteSpace(valor);
    }

    public static bool SuperaLongitudMaxima(string? valor, int longitudMaxima)
    {
        return !string.IsNullOrWhiteSpace(valor) && valor.Length > longitudMaxima;
    }

    public static bool NoAlcanzaLongitudMinima(string? valor, int longitudMinima)
    {
        return !string.IsNullOrWhiteSpace(valor) && valor.Length < longitudMinima;
    }

    public static bool NoEsLongitudExacta(string? valor, int longitudMinima)
    {
        return !string.IsNullOrWhiteSpace(valor) && valor.Length != longitudMinima;
    }
}
