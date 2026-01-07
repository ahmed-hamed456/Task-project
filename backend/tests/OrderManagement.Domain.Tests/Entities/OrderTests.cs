using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.ValueObjects;
using Xunit;

namespace OrderManagement.Domain.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void CreateOrder_WithValidCustomer_ShouldSucceed()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");

        // Act
        var order = new Order(customer);

        // Assert
        order.Customer.Should().Be(customer);
        order.Status.Should().Be(OrderStatus.Pending);
        order.Items.Should().BeEmpty();
    }

    [Fact]
    public void AddItem_WhenOrderNotCompleted_ShouldAddItem()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        var quantity = new Quantity(2);

        // Act
        order.AddItem(product, quantity);

        // Assert
        order.Items.Should().HaveCount(1);
        order.Items.First().ProductId.Should().Be(product.Id);
        order.Items.First().Quantity.Value.Should().Be(2);
    }

    [Fact]
    public void AddItem_WhenOrderCompleted_ShouldThrowException()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        var quantity = new Quantity(1);
        
        order.AddItem(product, quantity);
        order.MarkAsCompleted();

        var newProduct = new Product("Mouse", "Description", new Money(29.99m));
        Action act = () => order.AddItem(newProduct, new Quantity(1));

        // Act & Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot modify a completed order.");
    }

    [Fact]
    public void RemoveItem_WhenOrderNotCompleted_ShouldRemoveItem()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        var quantity = new Quantity(1);
        
        order.AddItem(product, quantity);

        // Act
        order.RemoveItem(product.Id);

        // Assert
        order.Items.Should().BeEmpty();
    }

    [Fact]
    public void RemoveItem_WhenOrderCompleted_ShouldThrowException()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        var quantity = new Quantity(1);
        
        order.AddItem(product, quantity);
        order.MarkAsCompleted();

        Action act = () => order.RemoveItem(product.Id);

        // Act & Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot modify a completed order.");
    }

    [Fact]
    public void CalculateTotal_WithMultipleItems_ShouldReturnCorrectSum()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);
        
        var product = new Product("Laptop", "Description", new Money(1000m));
        
        order.AddItem(product, new Quantity(2)); // 2000

        // Act
        var total = order.CalculateTotal();

        // Assert
        total.Amount.Should().Be(2000m);
    }

    [Fact]
    public void MarkAsCompleted_WithItems_ShouldChangeStatus()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);
        var product = new Product("Laptop", "Description", new Money(1299.99m));
        
        order.AddItem(product, new Quantity(1));

        // Act
        order.MarkAsCompleted();

        // Assert
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void MarkAsCompleted_WithoutItems_ShouldThrowException()
    {
        // Arrange
        var customer = new Customer("John Doe", "john@example.com");
        var order = new Order(customer);

        Action act = () => order.MarkAsCompleted();

        // Act & Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot complete an order with no items.");
    }
}
