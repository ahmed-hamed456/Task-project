using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.ValueObjects;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(OrderManagementDbContext context)
    {
        context.Database.EnsureCreated();

        // Check if database is already seeded
        if (context.Products.Any())
        {
            return;
        }

        // Seed Products
        var products = new[]
        {
            new Product("Laptop", "High-performance laptop for professionals", new Money(1299.99m)),
            new Product("Wireless Mouse", "Ergonomic wireless mouse with long battery life", new Money(29.99m)),
            new Product("Mechanical Keyboard", "RGB mechanical keyboard with Cherry MX switches", new Money(129.99m)),
            new Product("USB-C Hub", "7-in-1 USB-C hub with HDMI and Ethernet ports", new Money(49.99m)),
            new Product("Laptop Stand", "Adjustable aluminum laptop stand", new Money(39.99m)),
            new Product("Webcam", "1080p webcam with built-in microphone", new Money(79.99m)),
            new Product("Headphones", "Noise-canceling over-ear headphones", new Money(199.99m)),
            new Product("External SSD", "1TB portable external SSD", new Money(149.99m)),
            new Product("Monitor", "27-inch 4K UHD monitor", new Money(399.99m)),
            new Product("Desk Lamp", "LED desk lamp with adjustable brightness", new Money(34.99m))
        };

        context.Products.AddRange(products);

        // Seed Customers
        var customers = new[]
        {
            new Customer("John Doe", "john.doe@example.com"),
            new Customer("Jane Smith", "jane.smith@example.com"),
            new Customer("Michael Johnson", "michael.johnson@example.com"),
            new Customer("Emily Williams", "emily.williams@example.com"),
            new Customer("David Brown", "david.brown@example.com")
        };

        context.Customers.AddRange(customers);

        context.SaveChanges();
    }
}
