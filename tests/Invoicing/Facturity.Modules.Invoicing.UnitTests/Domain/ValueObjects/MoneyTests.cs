using Facturity.Modules.Invoicing.Domain;

namespace Facturity.Modules.Invoicing.UnitTests.Domain;

public class MoneyExtensionsTests
{
    private record Item(Money Amount);

    [Fact]
    public void Sum_WithEmptyCollection_ShouldReturnZero()
    {
        var items = Array.Empty<Item>();

        var result = items.Sum(i => i.Amount);

        Assert.Equal(Money.Zero, result);
    }

    [Fact]
    public void Sum_WithSingleItem_ShouldReturnThatAmount()
    {
        var items = new[] { new Item(Build.GetMoney(10.50m)) };

        Assert.Equal(Build.GetMoney(10.50m), items.Sum(i => i.Amount));
    }

    [Fact]
    public void Sum_WithSeveralItems_ShouldReturnTotal()
    {
        var items = new[]
        {
            new Item(Build.GetMoney(10.50m)),
            new Item(Build.GetMoney(0.25m)),
            new Item(Build.GetMoney(5.00m))
        };

        Assert.Equal(Build.GetMoney(15.75m), items.Sum(i => i.Amount));
    }

    [Fact]
    public void Sum_ShouldUseTheSelector()
    {
        var items = new[] { (A: Build.GetMoney(1m), B: Build.GetMoney(100m)), (A: Build.GetMoney(2m), B: Build.GetMoney(200m)) };

        Assert.Equal(Build.GetMoney(3m), items.Sum(i => i.A));
        Assert.Equal(Build.GetMoney(300m), items.Sum(i => i.B));
    }

    [Fact]
    public void Sum_ShouldNotDependOnOrder()
    {
        var a = new[] { new Item(Build.GetMoney(1.10m)), new Item(Build.GetMoney(2.20m)), new Item(Build.GetMoney(3.30m)) };
        var b = a.Reverse().ToArray();

        Assert.Equal(a.Sum(i => i.Amount), b.Sum(i => i.Amount));
    }
}

public class MoneyTests
{
    [Theory]
    [InlineData(145.34)]
    [InlineData(43)]
    [InlineData(1345.45)]
    [InlineData(000434342341345.45)]
    public void Create_PositiveAmount_ReturnsSuccessMoneyResult(decimal amount)
    {
        var result = Money.Create(amount);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(amount, result.Value.Amount);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(-124.63)]
    [InlineData(0)]
    public void Create_NegativeOrZeroAmount_ReturnsFailureResult(decimal amount)
    {
        var result = Money.Create(amount);
        
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(MoneyError.InvalidAmountNegativeOrZero ,result.Errors[0]);
    }
    
    [Theory]
    [InlineData(145.345, 12.455, 157.810000)]
    [InlineData(43.006, 17.001, 60.01000000)]
    [InlineData(134.45, 755.43, 889.88)]
    [InlineData(1345.45, 44.345, 1389.8)]
    public void SumOperation_ValidMoneyObjectsSum_ReturnsSuccessMoney(decimal firstMoneyAmount, decimal secondMoneyAmount, decimal expectedSum)
    {
        var firstMoney = Money.Create(firstMoneyAmount).Value;
        var secondMoney = Money.Create(secondMoneyAmount).Value;
        var expectedSumMoney = Money.Create(expectedSum).Value;
        
        var resultMoney = firstMoney + secondMoney;
        
        Assert.IsType<Money>(resultMoney);
        Assert.Equal(expectedSumMoney, resultMoney);
    }
}