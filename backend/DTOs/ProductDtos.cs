using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class ProductFilterDto
    {
        public string? Name { get; set; }
        public string? Sku { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int? CurrentStock { get; set; } // Optional, defaults to 0
        public int MinimumStockLevel { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int MinimumStockLevel { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }

    public class StockUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
        public bool IsAddition { get; set; }
    }
}
