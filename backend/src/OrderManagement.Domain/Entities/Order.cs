using OrderManagement.Domain.Enums;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public int Id { get; private set; }
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items;
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    private Order() 
    {
        Customer = null!;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
    }

    public Order(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        Customer = customer;
        CustomerId = customer.Id;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Product product, Quantity quantity)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot modify a completed order.");

        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (quantity == null)
            throw new ArgumentNullException(nameof(quantity));

        // Check if product already exists in order
        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            var newQuantity = new Quantity(existingItem.Quantity.Value + quantity.Value);
            existingItem.UpdateQuantity(newQuantity);
        }
        else
        {
            var orderItem = new OrderItem(product, quantity);
            _items.Add(orderItem);
        }
    }

    public void RemoveItem(int productId)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot modify a completed order.");

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            throw new InvalidOperationException($"Product with ID {productId} not found in order.");

        _items.Remove(item);
    }

    public Money CalculateTotal()
    {
        if (!_items.Any())
            return new Money(0);

        var total = _items.First().CalculateLineTotal();
        foreach (var item in _items.Skip(1))
        {
            total = total.Add(item.CalculateLineTotal());
        }

        return total;
    }

    public void MarkAsCompleted()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Order is already completed.");

        if (!_items.Any())
            throw new InvalidOperationException("Cannot complete an order with no items.");

        Status = OrderStatus.Completed;
    }
}
