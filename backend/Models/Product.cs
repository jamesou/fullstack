using System;

namespace backend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Sku { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStockLevel { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public bool IsLowStock => CurrentStock <= MinimumStockLevel;

        // Business rules validation
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Sku) &&
                   Price >= 0 &&
                   MinimumStockLevel >= 0;
        }
    }
}
