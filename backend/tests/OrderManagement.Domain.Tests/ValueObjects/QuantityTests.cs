using FluentAssertions;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.Tests.ValueObjects;

public class QuantityTests
{
    [Fact]
    public void CreateQuantity_WithValidValue_ShouldSucceed()
    {
        // Arrange & Act
        var quantity = new Quantity(5);

        // Assert
        quantity.Value.Should().Be(5);
    }

    [Fact]
    public void CreateQuantity_WithZero_ShouldThrowException()
    {
        // Arrange
        Action act = () => new Quantity(0);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero.*");
    }

    [Fact]
    public void CreateQuantity_WithNegativeValue_ShouldThrowException()
    {
        // Arrange
        Action act = () => new Quantity(-5);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero.*");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var quantity1 = new Quantity(5);
        var quantity2 = new Quantity(5);

        // Act & Assert
        quantity1.Should().Be(quantity2);
        (quantity1 == quantity2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var quantity1 = new Quantity(5);
        var quantity2 = new Quantity(10);

        // Act & Assert
        quantity1.Should().NotBe(quantity2);
        (quantity1 != quantity2).Should().BeTrue();
    }
}
