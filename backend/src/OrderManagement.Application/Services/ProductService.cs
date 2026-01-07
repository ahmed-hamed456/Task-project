using FluentValidation;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<CreateProductRequest> _createProductValidator;

    public ProductService(
        IProductRepository productRepository,
        IValidator<CreateProductRequest> createProductValidator)
    {
        _productRepository = productRepository;
        _createProductValidator = createProductValidator;
    }

    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var validationResult = await _createProductValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var money = new Money(request.Price, request.Currency);
        var product = new Product(request.Name, request.Description, money);

        var createdProduct = await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return new ProductResponse
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price.Amount,
            Currency = createdProduct.Price.Currency
        };
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price.Amount,
            Currency = p.Price.Currency
        });
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return null;

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Amount,
            Currency = product.Price.Currency
        };
    }
}
