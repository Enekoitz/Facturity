namespace Facturity.Modules.Customer.Domain;

public static class ClienteParametros
{
    public const int AliasMaxLength = 50;
    public const int NombreMaxLength = 50;
    public const int ApellidoMaxLength = 50;
    public const int RazonSocialMaxLength = 150;
}

public static class DomicilioParametros
{
    public const int CodigoPostalLength = 5;
    public const int DireccionMaxLength = 200;
    public const int MunicipioMaxLength = 100;
    public const int ProvinciaMaxLength = 50;
    public const int RegionMaxLength = 50;
    public const int PaisMaxLength = 60;
}

public static class NifEspanolParametros
{
    public const int ValorMaxLength = 9;  
}

public static class IdOtroExtranjeroParametros
{
    public const int ValorMaxLength = 20;  
    public const int CodigoPaisMaxLength = 2;
}