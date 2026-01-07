using FluentAssertions;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void CreateMoney_WithValidAmount_ShouldSucceed()
    {
        // Arrange & Act
        var money = new Money(100m, "USD");

        // Assert
        money.Amount.Should().Be(100m);
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void CreateMoney_WithNegativeAmount_ShouldThrowException()
    {
        // Arrange
        Action act = () => new Money(-10m);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Money amount cannot be negative.*");
    }

    [Fact]
    public void Add_WithSameCurrency_ShouldReturnCorrectSum()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(50m, "USD");

        // Act
        var result = money1.Add(money2);

        // Assert
        result.Amount.Should().Be(150m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Add_WithDifferentCurrency_ShouldThrowException()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(50m, "EUR");

        Action act = () => money1.Add(money2);

        // Act & Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add money with different currencies.");
    }

    [Fact]
    public void Multiply_WithPositiveMultiplier_ShouldReturnCorrectResult()
    {
        // Arrange
        var money = new Money(100m, "USD");

        // Act
        var result = money.Multiply(3);

        // Assert
        result.Amount.Should().Be(300m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(100m, "USD");

        // Act & Assert
        money1.Should().Be(money2);
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new Money(100m, "USD");
        var money2 = new Money(200m, "USD");

        // Act & Assert
        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }
}
