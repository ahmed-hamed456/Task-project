using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }

    // EF Core constructor
    private Product() 
    {
        Name = string.Empty;
        Description = string.Empty;
        Price = new Money(0);
    }

    public Product(string name, string description, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        if (price.Amount < 0)
            throw new ArgumentException("Product price must be greater than or equal to zero.", nameof(price));

        Name = name;
        Description = description ?? string.Empty;
        Price = price;
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount < 0)
            throw new ArgumentException("Product price must be greater than or equal to zero.", nameof(newPrice));

        Price = newPrice;
    }
}
