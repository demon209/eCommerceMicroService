using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        //Override lại OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                // Cấu hình kiểu dữ liệu chính xác cho Price
                entity.Property(p => p.Price)
                      .HasPrecision(18, 2); // 18 chữ số, trong đó 2 là sau dấu thập phân
            });
        }
    }
}
