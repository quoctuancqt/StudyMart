using System;
using StudyMart.ApiService.Data.Entities;
using StudyMart.ApiService.Extensions;

namespace StudyMart.ApiService.Data;

public class AppDbContextSeed : IDbSeeder<AppDbContext>
{
    public async Task SeedAsync(AppDbContext context)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Electronics", Description = "Electronic gadgets and accessories" },
                new() { Name = "Books", Description = "Books for all ages" },
                new() { Name = "Clothing", Description = "Clothing for all ages" }
            };

            await context.Categories.AddRangeAsync(categories);

            var products = new List<Product>
            {
                new() { Name = "Laptop", Description = "A laptop for all your needs", Price = 1000, CategoryId = 1, ImageUrl = "https://picsum.photos/300/300" },
                new() { Name = "Mobile Phone", Description = "A mobile phone for all your needs", Price = 500, CategoryId = 1, ImageUrl = "https://picsum.photos/300/300"},
                new() { Name = "Headphones", Description = "Headphones for all your needs", Price = 100, CategoryId = 1, ImageUrl = "https://picsum.photos/300/300"},
                new() { Name = "Book", Description = "A book for all your needs", Price = 10, CategoryId = 2, ImageUrl = "https://picsum.photos/300/300"},
                new() { Name = "T-Shirt", Description = "A t-shirt for all your needs", Price = 20, CategoryId = 3, ImageUrl = "https://picsum.photos/300/300"}
            };

            await context.Products.AddRangeAsync(products);
        }

        await context.SaveChangesAsync();
    }
}
