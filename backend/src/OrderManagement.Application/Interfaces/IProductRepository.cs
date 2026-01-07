using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> AddAsync(Product product);
    Task SaveChangesAsync();
}
