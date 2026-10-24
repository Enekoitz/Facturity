namespace Facturity.Modules.Customer.Domain;

public static class Parameters
{
    public static class Customer
    {
        public const int AliasMaxLength = 50;
        public const int NameMaxLength = 50;
        public const int SurnameMaxLength = 50;
        public const int LegalNameMaxLength = 150;
    }

    public static class Address
    {
        public const int PostalCodeExactLength = 5;
        public const int AddressMaxLength = 200;
        public const int CityMaxLength = 100;
        public const int StateMaxLength = 50;
        public const int RegionMaxLength = 50;
        public const int CountryMaxLength = 60;
    }

    public static class SpanishTaxId
    {
        public const int ValueMaxLength = 9;  
    }

    public static class OtherTaxId
    {
        public const int ValueMaxLength = 20;  
        public const int CountryCodeExactLength = 2;
    }
}