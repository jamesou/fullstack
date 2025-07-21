using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class PostgresProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public PostgresProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => p.CurrentStock <= p.MinimumStockLevel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> FilterProductsAsync(
            string name,
            string sku,
            bool? isActive,
            int? minStock,
            int? maxStock)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => EF.Functions.ILike(p.Name, $"%{name}%"));

            if (!string.IsNullOrWhiteSpace(sku))
                query = query.Where(p => EF.Functions.ILike(p.Sku, $"%{sku}%"));

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            if (minStock.HasValue)
                query = query.Where(p => p.CurrentStock >= minStock.Value);

            if (maxStock.HasValue)
                query = query.Where(p => p.CurrentStock <= maxStock.Value);

            return await query.ToListAsync();
        }
    }
}
