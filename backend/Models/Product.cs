using System;

namespace backend.Models
{
    public class Product
    {
        public Guid Id { get; set; } // Unique identifier
        public string Name { get; set; } // Product name
        public string SKU { get; set; } // Stock Keeping Unit, unique code
        public string Description { get; set; } // Product description
        public decimal Price { get; set; } // Product price
        public int CurrentStock { get; set; } // Current stock quantity
        public int MinimumStockLevel { get; set; } // Minimum stock level for warning
        public DateTime LastUpdated { get; set; } // Last updated time
        public bool IsActive { get; set; } // Is the product active/available (soft delete)
    }
}
