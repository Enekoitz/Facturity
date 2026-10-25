using Facturity.Modules.Customer.Domain;
using Xunit;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class PostalCodeTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Create_Empty_ReturnsFailure(string postalCode)
    {
        var result = PostalCode.Crear(postalCode);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Create_NotOnlyDigits_ReturnsFailure()
    {
        const string postalCode = "12f&3";

        var result = PostalCode.Crear(postalCode);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Create_NotExactDigits_ReturnsFailure()
    {
        const string postalCode = "123456";

        var result = PostalCode.Crear(postalCode);

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void Create_Valid_ReturnsSuccessfulPostalCode()
    {
        const string postalCode = "02344";

        var result = PostalCode.Crear(postalCode);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(postalCode, result.Value.Valor);
    }
}