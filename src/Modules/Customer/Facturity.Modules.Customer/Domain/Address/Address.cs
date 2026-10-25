using Facturity.Modules.SharedKernel.Domain;

namespace Facturity.Modules.Customer.Domain;

public sealed record Address
{
    public string Street { get; private set; }
    public PostalCode? PostalCode { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Region { get; private set; }
    public string? Country { get; private set; }

    protected Address() { }

    private Address(
        string street
        , PostalCode? postalCode
        , string? city
        , string? state
        , string? region
        , string? country)
    {
        Street = street;
        PostalCode = postalCode;
        City = city;
        State = state;
        Region = region;
        Country = country;
    }

    public static Result<Address> Crear(
        string street
        , string? postalCode
        , string? city                       
        , string? state
        , string? region
        , string? country)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(street))
            errors.Add(AddressErrors.StreetEmpty);

        city = string.IsNullOrWhiteSpace(city) ? null : city.Trim();
        state = string.IsNullOrWhiteSpace(state) ? null : state.Trim();
        region = string.IsNullOrWhiteSpace(region) ? null : region.Trim();
        country = string.IsNullOrWhiteSpace(country) ? null : country.Trim();

        PostalCode? postalCodeValue = null;
        if (!string.IsNullOrWhiteSpace(postalCode))
        {
            var postalCodeResult = PostalCode.Crear(postalCode);
            if (postalCodeResult.IsFailure)
                errors.AddRange(postalCodeResult.Errors);
            else
                postalCodeValue = postalCodeResult.Value;
        }

        if (errors.Count > 0)
            return Result.Fail<Address>(errors);

        street = street.Trim();

        return Result.Ok(new Address(
            street,
            postalCodeValue, 
            city, 
            state, 
            region, 
            country));
    }
}