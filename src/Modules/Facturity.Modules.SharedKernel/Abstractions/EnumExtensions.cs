namespace Facturity.Modules.SharedKernel.Abstractions;

public static class EnumExtensions
{
    public static TEnum? ParseToNullableEnum<TEnum>(this string? valor, bool ignoreCase = true) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(valor))
            return null;

        return Enum.TryParse<TEnum>(valor, ignoreCase, out var resultado)
            ? resultado
            : null;
    }
}