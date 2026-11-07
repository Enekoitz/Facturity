using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class IssuanceInfoTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 7, 12, 0, 0, TimeSpan.Zero);
    
    [Fact]
    public void Issue_WhenIssueDateIsValid_ReturnsObjectCorrectly()
    {
        var issuedAtUtc = Now.AddTicks(-1);
        var result = IssuanceInfo.Create(issuedAtUtc, Now);
 
        Assert.True(result.IsSuccess);
        Assert.Equal(issuedAtUtc, result.Value.IssuedAtUtc);
        Assert.Equal(null!, result.Value.Number);
    }
    
    [Fact]
    public void Issue_WhenIssueDateIsMinValue_FailsWithIssueDateInvalidOnly()
    {
        
        var result = IssuanceInfo.Create(DateTimeOffset.MinValue, Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(IssuanceInfoError.IssueDateInvalid, Assert.Single(result.Errors));
        Assert.DoesNotContain(IssuanceInfoError.IssueDateIsFuture, result.Errors);
    }
 
    [Fact]
    public void Issue_WhenIssueDateIsInTheFuture_FailsWithIssueDateFuture()
    {
        var result = IssuanceInfo.Create(Now.AddDays(1), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(IssuanceInfoError.IssueDateIsFuture, Assert.Single(result.Errors));
    }
 
    [Fact]
    public void Issue_WhenIssueDateIsOneTickAfterNow_FailsWithIssueDateFuture()
    {
        var result = IssuanceInfo.Create(Now.AddTicks(1), Now);
 
        Assert.True(result.IsFailure);
        Assert.Equal(IssuanceInfoError.IssueDateIsFuture, Assert.Single(result.Errors));
    }
}