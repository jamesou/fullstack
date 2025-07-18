using backend.Models;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // In-memory product list for demonstration. Replace with database in production.
        private static readonly List<Product> Products = new();

        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] CreateProductDto dto)
        {
            if (Products.Any(p => p.SKU == dto.SKU))
                return Conflict(new { message = "SKU must be unique." });

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                SKU = dto.SKU,
                Description = dto.Description ?? string.Empty,
                Price = dto.Price,
                CurrentStock = dto.CurrentStock ?? 0,
                MinimumStockLevel = dto.MinimumStockLevel,
                LastUpdated = DateTime.UtcNow,
                IsActive = dto.IsActive
            };
            Products.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAllProducts(
            [FromQuery] string? name,
            [FromQuery] string? sku,
            [FromQuery] int? minStock,
            [FromQuery] int? maxStock,
            [FromQuery] bool? isActive,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder)
        {
            var query = Products.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(sku))
                query = query.Where(p => p.SKU == sku);
            if (minStock.HasValue)
                query = query.Where(p => p.CurrentStock >= minStock);
            if (maxStock.HasValue)
                query = query.Where(p => p.CurrentStock <= maxStock);
            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive);
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool desc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
                query = sortBy.ToLower() switch
                {
                    "name" => desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    "currentstock" => desc ? query.OrderByDescending(p => p.CurrentStock) : query.OrderBy(p => p.CurrentStock),
                    _ => query
                };
            }
            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(Guid id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(Guid id, [FromBody] UpdateProductDto dto)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            product.Name = dto.Name;
            product.Description = dto.Description ?? string.Empty;
            product.Price = dto.Price;
            product.MinimumStockLevel = dto.MinimumStockLevel;
            product.IsActive = dto.IsActive;
            product.LastUpdated = DateTime.UtcNow;
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            product.IsActive = false; // Soft delete
            product.LastUpdated = DateTime.UtcNow;
            return NoContent();
        }

        [HttpPost("{id}/add-stock")]
        public ActionResult<Product> AddStock(Guid id, [FromBody] StockOperationDto dto)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            product.CurrentStock += dto.Quantity;
            product.LastUpdated = DateTime.UtcNow;
            return Ok(product);
        }

        [HttpPost("{id}/remove-stock")]
        public ActionResult<Product> RemoveStock(Guid id, [FromBody] StockOperationDto dto)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            if (product.CurrentStock < dto.Quantity)
                return BadRequest(new { message = "Not enough stock to remove." });
            product.CurrentStock -= dto.Quantity;
            product.LastUpdated = DateTime.UtcNow;
            return Ok(product);
        }

        [HttpGet("low-stock")]
        public ActionResult<IEnumerable<Product>> GetLowStockProducts()
        {
            var lowStock = Products.Where(p => p.CurrentStock < p.MinimumStockLevel).ToList();
            return Ok(lowStock);
        }
    }
}
