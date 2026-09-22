namespace Facturity.Modules.SharedKernel.Abstractions;

public static class Utilities
{
    public static string GetRandomLength(int longitud)
    {
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
        // Toma elementos aleatorios del conjunto y crea el string directamente
        return new string(Random.Shared.GetItems(caracteres.ToCharArray(), longitud));
    }
}