using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudyMart.ApiService.Authorization;
using StudyMart.ApiService.Data;
using StudyMart.ApiService.Data.Entities;
using StudyMart.ApiService.Filters;
using StudyMart.Contract.Product;

namespace StudyMart.ApiService.Features.Product;

internal static class ProductApi
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/products")
            .WithTags("Products");

        group.RequireAuthorization();

        group.WithParameterValidation(typeof(CreateOrUpdateProductDto));

        group.MapGet("/", async (AppDbContext db) => await db.Products.AsNoTracking().Select(p => p.ToDto()).ToListAsync());

        group.MapGet("/{id}", async Task<Results<Ok<ProductDto>, NotFound>> (int id, AppDbContext db) =>
        {
            return await db.Products.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == id)
                is { } product
                ? TypedResults.Ok(product.ToDto())
                : TypedResults.NotFound();
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, CreateOrUpdateProductDto product, AppDbContext db) =>
        {
            var affected = await db.Products
                .Where(model => model.ProductId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Name, product.Name)
                    .SetProperty(m => m.Description, product.Description)
                    .SetProperty(m => m.Price, product.Price)
                    .SetProperty(m => m.ImageUrl, product.ImageUrl)
                    .SetProperty(m => m.CategoryId, product.CategoryId)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        });
        
        group.MapPut("/{id}/reviews", async Task<Results<Ok, NotFound>> (int id, CreateReviewDto review, AppDbContext db, CurrentUser currentUser) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product is null) return TypedResults.NotFound();
            
            var newReview = new Review
            {
                ProductId = id,
                Rating = review.Rating,
                Comment = review.Comment,
                UserId = currentUser.UserId!
            };
            db.Reviews.Add(newReview);
            await db.SaveChangesAsync();
            return TypedResults.Ok();
        });

        group.MapPost("/", async (CreateOrUpdateProductDto newProduct, AppDbContext db) =>
        {
            var product = new Data.Entities.Product
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                ImageUrl = newProduct.ImageUrl,
                CategoryId = newProduct.CategoryId
            };
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/products/{product.ProductId}", product);
        });

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var rowsAffected = await db.Products.Where(t => t.CategoryId == id)
                .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(t => t.IsDeleted, true));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }
}