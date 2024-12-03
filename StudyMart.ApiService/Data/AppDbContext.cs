using Microsoft.EntityFrameworkCore;
using StudyMart.ApiService.Data.Entities;

namespace StudyMart.ApiService.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>().HasQueryFilter(r => !r.IsDeleted);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.CartItems)
                .WithOne(sc => sc.Product)
                .HasForeignKey(sc => sc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Product>().HasQueryFilter(r => !r.IsDeleted);

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasMany(sc => sc.CartItems)
                .WithOne(p => p.ShoppingCart)
                .HasForeignKey(sc => sc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(oi => oi.ShoppingCart)
                .WithMany(o => o.CartItems)
                .HasForeignKey(oi => oi.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
        });
            
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        base.OnModelCreating(modelBuilder);
    }

    internal async Task SeedData()
    {
        // Seed data for Categories and Products
        var categories = new List<Category>
        {
            new() { Name = "Electronics", Description = "Electronic gadgets and accessories" },
            new() { Name = "Books", Description = "Books for all ages" },
            new() { Name = "Clothing", Description = "Clothing for all ages" }
        };

        await Categories.AddRangeAsync(categories);

        var products = new List<Product>
        {
            new() { Name = "Laptop", Description = "A laptop for all your needs", Price = 1000, CategoryId = 1 },
            new() { Name = "Mobile Phone", Description = "A mobile phone for all your needs", Price = 500, CategoryId = 1 },
            new() { Name = "Headphones", Description = "Headphones for all your needs", Price = 100, CategoryId = 1 },
            new() { Name = "Book", Description = "A book for all your needs", Price = 10, CategoryId = 2 },
            new() { Name = "T-Shirt", Description = "A t-shirt for all your needs", Price = 20, CategoryId = 3 }
        };

        await Products.AddRangeAsync(products);

        await SaveChangesAsync();
    }
}
