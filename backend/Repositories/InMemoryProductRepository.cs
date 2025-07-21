using backend.Interfaces;
using backend.Models;
using System.Collections.Concurrent;

namespace backend.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<int, Product> _products = new();
        private int _nextId = 1;

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.FromResult(_products.Values);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            _products.TryGetValue(id, out var product);
            return await Task.FromResult(product);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            product.Id = _nextId++;
            if (_products.TryAdd(product.Id, product))
                return await Task.FromResult(product);
            throw new Exception("Failed to create product");
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            if (_products.TryGetValue(product.Id, out _))
            {
                _products[product.Id] = product;
                return await Task.FromResult(product);
            }
            throw new KeyNotFoundException($"Product with ID {product.Id} not found");
        }

        public async Task DeleteAsync(int id)
        {
            if (!_products.TryRemove(id, out _))
                throw new KeyNotFoundException($"Product with ID {id} not found");
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Task.FromResult(_products.ContainsKey(id));
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await Task.FromResult(
                _products.Values.Where(p => p.CurrentStock <= p.MinimumStockLevel)
            );
        }

        public async Task<IEnumerable<Product>> FilterProductsAsync(
            string name,
            string sku,
            bool? isActive,
            int? minStock,
            int? maxStock)
        {
            var query = _products.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(sku))
                query = query.Where(p => p.Sku.Contains(sku, StringComparison.OrdinalIgnoreCase));

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            if (minStock.HasValue)
                query = query.Where(p => p.CurrentStock >= minStock.Value);

            if (maxStock.HasValue)
                query = query.Where(p => p.CurrentStock <= maxStock.Value);

            return await Task.FromResult(query.ToList());
        }
    }
}
