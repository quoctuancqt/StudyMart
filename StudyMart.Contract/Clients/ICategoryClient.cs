using Refit;
using StudyMart.Contract.Category;

namespace StudyMart.Contract.Clients;

public interface ICategoryClient
{
    [Get("/api/categories")]
    Task<List<CategoryDto>> GetCategoriesAsync();
    
    [Get("/api/categories/{id}")]
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    
    [Post("/api/categories")]
    Task<HttpResponseMessage> AddCategoryAsync(CategoryDto category);
    
    [Put("/api/categories/{id}")]
    Task<HttpResponseMessage> UpdateCategoryAsync(CategoryDto category);
    
    [Delete("/api/categories/{id}")]
    Task<HttpResponseMessage> DeleteCategoryAsync(int id);
}