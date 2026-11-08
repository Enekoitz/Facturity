namespace Facturity.Modules.SharedKernel.Domain;

public static class Utilities
{
    ///<summary>
    ///Returns the number of significant decimal places of a 'decimal' value, removing redundant trailing zeros.
    ///</summary>
    public static int GetSignificantDecimalPlaces(decimal value)
    {
        int scale = value.Scale;

        while (scale > 0 && decimal.Round(value, scale - 1) == value)
            scale--;

        return scale;
    }
    
    public static string GetRandomLength(int longitud)
    {
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
        // Toma elementos aleatorios del conjunto y crea el string directamente
        return new string(Random.Shared.GetItems(caracteres.ToCharArray(), longitud));
    }
}