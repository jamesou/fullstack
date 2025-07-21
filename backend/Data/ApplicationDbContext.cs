using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products", "warehouse");
                
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                
                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Sku)
                    .HasColumnName("sku")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.Sku)
                    .IsUnique();

                entity.Property(e => e.Description)
                    .HasColumnName("description");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.CurrentStock)
                    .HasColumnName("current_stock");

                entity.Property(e => e.MinimumStockLevel)
                    .HasColumnName("minimum_stock_level");

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active");
            });
        }
    }
}
