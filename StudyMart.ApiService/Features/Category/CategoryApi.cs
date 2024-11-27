using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudyMart.ApiService.Data;
using StudyMart.ApiService.Filters;
using StudyMart.Contract.Category;
using CategoryModel = StudyMart.ApiService.Data.Entities.Category;

namespace StudyMart.ApiService.Features.Category;

internal static class CategoryApi
{
    public static RouteGroupBuilder MapCategories(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/categories");

        group.WithTags("Categories");

        group.RequireAuthorization();

        // Validate the parameters
        group.WithParameterValidation(typeof(CreateOrUpdateCategoryDto));

        group.MapGet("/", async (AppDbContext db) => await db.Categories.AsNoTracking().Select(c => c.ToDto()).ToListAsync());

        group.MapGet("/{id}", async Task<Results<Ok<CategoryDto>, NotFound>> (AppDbContext db, int id) =>
        {
            return await db.Categories.FindAsync(id) switch
            {
                { } category => TypedResults.Ok(new CategoryDto(category.CategoryID, category.Name)),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<CreateOrUpdateCategoryDto>> (AppDbContext db, CreateOrUpdateCategoryDto newCategory) =>
        {
            var category = new CategoryModel { Name = newCategory.Name };
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/categories/{category.CategoryID}", newCategory);
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (AppDbContext db, int id, CreateOrUpdateCategoryDto category) =>
        {
            var rowsAffected = await db.Categories.Where(t => t.CategoryID == id)
                                             .ExecuteUpdateAsync(updates =>
                                                updates.SetProperty(t => t.Name, category.Name));

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
