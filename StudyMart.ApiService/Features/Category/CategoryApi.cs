using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudyMart.ApiService.Authorization;
using StudyMart.ApiService.Data;
using StudyMart.ApiService.Filters;
using CategoryModel = StudyMart.ApiService.Data.Entities.Category;

namespace StudyMart.ApiService.Features.Category;

internal static class CategoryApi
{
    public static RouteGroupBuilder MapCategories(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/categories");

        group.WithTags("Categories");

        // Add security requirements, all incoming requests to this API *must*
        // be authenticated with a valid user.
        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        // group.RequirePerUserRateLimit();

        // Validate the parameters
        group.WithParameterValidation(typeof(CategoryModel));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Categories.AsNoTracking().ToListAsync();
        });

        group.MapGet("/{id}", async Task<Results<Ok<CategoryModel>, NotFound>> (AppDbContext db, int id) =>
        {
            return await db.Categories.FindAsync(id) switch
            {
                CategoryModel category => TypedResults.Ok(category),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<CategoryModel>> (AppDbContext db, CategoryModel newCategory) =>
        {
            db.Categories.Add(newCategory);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/categories/{newCategory.CategoryID}", newCategory);
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (AppDbContext db, int id, CategoryModel category) =>
        {
            if (id != category.CategoryID)
            {
                return TypedResults.BadRequest();
            }

            var rowsAffected = await db.Categories.Where(t => t.CategoryID == id)
                                             .ExecuteUpdateAsync(updates =>
                                                updates.SetProperty(t => t.IsDeleted, category.IsDeleted)
                                                       .SetProperty(t => t.Name, category.Name));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (AppDbContext db, int id) =>
        {
            var rowsAffected = await db.Categories.Where(t => t.CategoryID == id)
                                             .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }
}
