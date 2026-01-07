namespace OrderManagement.Application.DTOs;

public class AddItemToOrderRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
