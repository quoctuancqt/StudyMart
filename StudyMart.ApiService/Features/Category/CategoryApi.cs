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
        var group = routes.MapGroup("/api/v{version:apiVersion}/categories")
            .WithApiVersionSet(routes.CreateApiVersionSet())
            .WithTags("Categories");

        group.RequireAuthorization(config => config.RequireRole("Administrator"));

        // Validate the parameters
        group.WithParameterValidation(typeof(CreateOrUpdateCategoryDto));

        group.MapGet("/", async (AppDbContext db) => await db.Categories.AsNoTracking().Select(c => c.ToDto()).ToListAsync());

        group.MapGet("/{id}", async Task<Results<Ok<CategoryDto>, NotFound>> (AppDbContext db, int id) =>
        {
            return await db.Categories.FindAsync(id) switch
            {
                { } category => TypedResults.Ok(category.ToDto()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<CategoryDto>> (AppDbContext db, CreateOrUpdateCategoryDto newCategory) =>
        {
            var category = new CategoryModel { Name = newCategory.Name, Description = newCategory.Description };
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/v{{version:apiVersion}}/categories/{category.CategoryId}", category.ToDto());
        });

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound, BadRequest<HttpValidationProblemDetails>>> (AppDbContext db, int id, CreateOrUpdateCategoryDto category) =>
        {
            var rowsAffected = await db.Categories.Where(t => t.CategoryId == id)
                                             .ExecuteUpdateAsync(updates =>
                                                updates.SetProperty(t => t.Name, category.Name)
                                                .SetProperty(t => t.Description, category.Description));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.NoContent();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, NoContent>> (AppDbContext db, int id) =>
        {
            var rowsAffected = await db.Categories.Where(t => t.CategoryId == id)
                .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(t => t.IsDeleted, true));

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.NoContent();
        });

        return group;
    }
}
