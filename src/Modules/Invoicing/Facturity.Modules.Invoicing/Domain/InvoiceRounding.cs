namespace Facturity.Modules.Invoicing.Domain;

internal static class InvoiceRounding
{
    internal const int Decimals = 2;
    internal const MidpointRounding Mode = MidpointRounding.AwayFromZero;

    internal static decimal Round(decimal value) =>
        Math.Round(value, Decimals, Mode);
}