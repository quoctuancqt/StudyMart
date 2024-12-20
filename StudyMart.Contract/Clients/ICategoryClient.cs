using Refit;
using StudyMart.Contract.Category;

namespace StudyMart.Contract.Clients;

public interface ICategoryClient
{
    [Get("/api/v1/categories")]
    Task<List<CategoryDto>> GetCategoriesAsync();
    
    [Get("/api/v1/categories/{id}")]
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    
    [Post("/api/v1/categories")]
    Task<HttpResponseMessage> AddCategoryAsync(CategoryDto category);
    
    [Put("/api/v1/categories/{id}")]
    Task<HttpResponseMessage> UpdateCategoryAsync(CategoryDto category);
    
    [Delete("/api/v1/categories/{id}")]
    Task<HttpResponseMessage> DeleteCategoryAsync(int id);
}