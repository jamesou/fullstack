using backend.DTOs;
using backend.Interfaces;
using backend.Models;
// Add this if ProductFilterDto is in another namespace
// using backend.DTOs; // Already present, ensure ProductFilterDto exists in this namespace

namespace backend.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(ProductFilterDto filters)
        {
            return await _productRepository.FilterProductsAsync(
                filters.Name,
                filters.Sku,
                filters.IsActive,
                filters.MinStock,
                filters.MaxStock
            );
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Add validation logic here
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required");
            if (string.IsNullOrWhiteSpace(product.Sku))
                throw new ArgumentException("Product SKU is required");
            if (product.Price < 0)
                throw new ArgumentException("Product price cannot be negative");
            if (product.MinimumStockLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative");

            return await _productRepository.CreateAsync(product);
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            product.Id = id;
            return await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var exists = await _productRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            await _productRepository.DeleteAsync(id);
        }

        public async Task<Product> UpdateStockAsync(int id, int quantity, bool isAddition)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            if (isAddition)
            {
                product.CurrentStock += quantity;
            }
            else
            {
                if (product.CurrentStock < quantity)
                    throw new InvalidOperationException("Cannot remove more stock than available");
                product.CurrentStock -= quantity;
            }

            return await _productRepository.UpdateAsync(product);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _productRepository.GetLowStockProductsAsync();
        }
    }
}
