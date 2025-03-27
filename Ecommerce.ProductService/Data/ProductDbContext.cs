using ECommerce.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
           Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            
            
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
        {
            Id = 1,
            Name = "Product 1",
            Price = 100,
            Quantity = 10
        });      
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
        {
            Id = 2,
            Name = "Product 2",
            Price = 200,
            Quantity = 20
        });      
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
        {
            Id = 3,
            Name = "Product 3",
            Price = 300,
            Quantity = 30
        });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ProductModel> Products { get; set; }
    }
}
