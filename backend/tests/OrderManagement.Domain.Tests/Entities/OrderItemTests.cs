using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.Tests.Entities;

public class OrderItemTests
{
    [Fact]
    public void CreateOrderItem_WithValidData_ShouldSucceed()
    {
        // Arrange
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        var quantity = new Quantity(2);

        // Act
        var orderItem = new OrderItem(product, quantity);

        // Assert
        orderItem.Product.Should().Be(product);
        orderItem.Quantity.Value.Should().Be(2);
        orderItem.UnitPrice.Should().Be(product.Price);
    }

    [Fact]
    public void CreateOrderItem_WithZeroQuantity_ShouldThrowException()
    {
        // Arrange
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        Action act = () => new Quantity(0);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero.*");
    }

    [Fact]
    public void CreateOrderItem_WithNegativeQuantity_ShouldThrowException()
    {
        // Arrange
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        Action act = () => new Quantity(-1);

        // Act & Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Quantity must be greater than zero.*");
    }

    [Fact]
    public void CalculateLineTotal_ShouldReturnPriceTimesQuantity()
    {
        // Arrange
        var product = new Product("Laptop", "Description", new Money(1000m));
        var quantity = new Quantity(3);
        var orderItem = new OrderItem(product, quantity);

        // Act
        var lineTotal = orderItem.CalculateLineTotal();

        // Assert
        lineTotal.Amount.Should().Be(3000m);
    }

    [Fact]
    public void UpdateQuantity_WithValidQuantity_ShouldUpdateQuantity()
    {
        // Arrange
        var product = new Product("Laptop", "Description", new Money(1000m));
        var orderItem = new OrderItem(product, new Quantity(2));
        var newQuantity = new Quantity(5);

        // Act
        orderItem.UpdateQuantity(newQuantity);

        // Assert
        orderItem.Quantity.Value.Should().Be(5);
    }
}
