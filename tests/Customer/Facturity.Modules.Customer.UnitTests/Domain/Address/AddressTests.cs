using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class AddressTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Create_WithInvalidStreet_ReturnsFailure(string street)
    {
        const string postalCode = "50001";
        const string? city = "Zaragoza";
        string? state = null;
        string? region = null;
        string? country = null;

        var result = Address.Create(street, postalCode, city, state, region, country);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_WithValidData_ReturnsSuccessfulAddress()
    {
        const string street = "Calle del Gran Capitán, 14";
        const string? postalCode = "14008";
        const string? city = "Córdoba";
        string? state = null;
        string? region = null;
        string? country = null;

        var result = Address.Create(street, postalCode, city, state, region, country);

        Assert.True(result.IsSuccess);
    }
}