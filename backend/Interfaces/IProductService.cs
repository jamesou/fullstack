using backend.DTOs;
using backend.Models;

namespace backend.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(ProductFilterDto filters);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task DeleteProductAsync(int id);
        Task<Product> UpdateStockAsync(int id, int quantity, bool isAddition);
        Task<IEnumerable<Product>> GetLowStockProductsAsync();
    }
}
