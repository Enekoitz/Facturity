using Facturity.Modules.Customer.Domain;

namespace Facturity.Modules.Customer.UnitTests.Domain;

public class SpanishTaxIdTests
{
    [Theory]
    [InlineData("12345678Z")] // DNI
    [InlineData("X1234567L")] // NIE
    [InlineData("A12345674")] // CIF
    public void Create_WithValidData_Success(string value)
    {
        var result = SpanishTaxId.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.Equal(value, result.Value.Value);
    }
    
    [Theory]
    [InlineData("A12345674")] // CIF
    public void Create_SubtypeCifMustBeLegalEntity_Success(string value)
    {
        var result = SpanishTaxId.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(true, result.Value.IsLegalEntity);
    }
    
    [Theory]
    [InlineData("12345678Z")] // DNI
    [InlineData("X1234567L")] // NIE
    public void Create_SubtypeDniNieMustNotBeLegalEntity_Success(string value)
    {
        var result = SpanishTaxId.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(false, result.Value.IsLegalEntity);
    }

    [Theory]
    [InlineData("11111678Z")] 
    [InlineData("X12F4567L")]
    [InlineData("ccccccccc")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidData_Failure(string value)
    {
        var result = SpanishTaxId.Create(value);

        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
    }
}