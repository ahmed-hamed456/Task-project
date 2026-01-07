using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void CreateProduct_WithValidData_ShouldSucceed()
    {
        // Arrange
        var name = "Laptop";
        var description = "High-performance laptop";
        var price = new Money(1299.99m);

        // Act
        var product = new Product(name, description, price);

        // Assert
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
    }

    [Fact]
    public void CreateProduct_WithNegativePrice_ShouldThrowException()
    {
        // Arrange
        var name = "Laptop";
        var description = "High-performance laptop";
        Action act = () => new Money(-100m);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Money amount cannot be negative.*");
    }

    [Fact]
    public void CreateProduct_WithEmptyName_ShouldThrowException()
    {
        // Arrange
        var price = new Money(1299.99m);
        Action act = () => new Product("", "Description", price);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Product name cannot be empty.*");
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
        // Arrange
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        var newPrice = new Money(1099.99m);

        // Act
        product.UpdatePrice(newPrice);

        // Assert
        product.Price.Should().Be(newPrice);
    }
}
