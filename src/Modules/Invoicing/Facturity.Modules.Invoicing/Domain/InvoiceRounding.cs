namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceRounding
{
    private const MidpointRounding Mode = MidpointRounding.AwayFromZero;

    internal static decimal Round(decimal value, int decimalPlaces = 2) 
        => Math.Round(value, decimalPlaces, Mode);
}