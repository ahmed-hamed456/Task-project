using FluentValidation;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateOrderRequest> _createOrderValidator;
    private readonly IValidator<AddItemToOrderRequest> _addItemValidator;

    public OrderService(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IValidator<CreateOrderRequest> createOrderValidator,
        IValidator<AddItemToOrderRequest> addItemValidator)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _createOrderValidator = createOrderValidator;
        _addItemValidator = addItemValidator;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found.");

        var order = new Order(customer);
        var createdOrder = await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return MapToOrderResponse(createdOrder);
    }

    public async Task<OrderResponse> AddItemToOrderAsync(int orderId, AddItemToOrderRequest request)
    {
        var validationResult = await _addItemValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order with ID {orderId} not found.");

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {request.ProductId} not found.");

        var quantity = new Quantity(request.Quantity);
        order.AddItem(product, quantity);

        await _orderRepository.SaveChangesAsync();

        return MapToOrderResponse(order);
    }

    public async Task<OrderResponse> RemoveItemFromOrderAsync(int orderId, int productId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order with ID {orderId} not found.");

        order.RemoveItem(productId);
        await _orderRepository.SaveChangesAsync();

        return MapToOrderResponse(order);
    }

    public async Task<OrderResponse?> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            return null;

        return MapToOrderResponse(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToOrderResponse);
    }

    public async Task<OrderResponse> CompleteOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new InvalidOperationException($"Order with ID {orderId} not found.");

        order.MarkAsCompleted();
        await _orderRepository.SaveChangesAsync();

        return MapToOrderResponse(order);
    }

    private OrderResponse MapToOrderResponse(Order order)
    {
        var total = order.CalculateTotal();

        return new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer.Name,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            Total = total.Amount,
            Currency = total.Currency,
            Items = order.Items.Select(i => new OrderItemResponse
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity.Value,
                UnitPrice = i.UnitPrice.Amount,
                LineTotal = i.CalculateLineTotal().Amount
            }).ToList()
        };
    }
}
