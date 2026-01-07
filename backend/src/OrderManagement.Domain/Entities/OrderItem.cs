using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

public class OrderItem
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public Product Product { get; private set; }
    public Quantity Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    // EF Core constructor
    private OrderItem() 
    {
        Product = null!;
        Quantity = new Quantity(1);
        UnitPrice = new Money(0);
    }

    public OrderItem(Product product, Quantity quantity)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (quantity == null)
            throw new ArgumentNullException(nameof(quantity));

        if (quantity.Value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        Product = product;
        ProductId = product.Id;
        Quantity = quantity;
        UnitPrice = product.Price;
    }

    public Money CalculateLineTotal()
    {
        return UnitPrice.Multiply(Quantity.Value);
    }

    public void UpdateQuantity(Quantity newQuantity)
    {
        if (newQuantity.Value <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        Quantity = newQuantity;
    }
}
