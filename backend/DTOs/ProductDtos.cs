using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string SKU { get; set; } = string.Empty;
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

    public class StockOperationDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
    }
}
