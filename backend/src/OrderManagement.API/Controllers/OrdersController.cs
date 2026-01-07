using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services;

namespace OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var order = await _orderService.CreateOrderAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Get an order by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id)
    {
        var order = await _orderService.GetOrderDetailsAsync(id);
        if (order == null)
            return NotFound(new { message = $"Order with ID {id} not found." });

        return Ok(order);
    }

    /// <summary>
    /// Add an item to an order
    /// </summary>
    [HttpPost("{id}/items")]
    public async Task<ActionResult<OrderResponse>> AddItem(int id, [FromBody] AddItemToOrderRequest request)
    {
        var order = await _orderService.AddItemToOrderAsync(id, request);
        return Ok(order);
    }

    /// <summary>
    /// Remove an item from an order
    /// </summary>
    [HttpDelete("{id}/items/{productId}")]
    public async Task<ActionResult<OrderResponse>> RemoveItem(int id, int productId)
    {
        var order = await _orderService.RemoveItemFromOrderAsync(id, productId);
        return Ok(order);
    }

    /// <summary>
    /// Mark an order as completed
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult<OrderResponse>> Complete(int id)
    {
        var order = await _orderService.CompleteOrderAsync(id);
        return Ok(order);
    }
}
