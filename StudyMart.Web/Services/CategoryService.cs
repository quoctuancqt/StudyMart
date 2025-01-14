using StudyMart.Contract.Category;
using StudyMart.Contract.Clients;

namespace StudyMart.Web.Services;

public class CategoryService(ApiClient apiClient)
{
    private readonly ICategoryClient _categoryClient = apiClient.CategoryClient;

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryClient.GetCategoriesAsync();
        return categories ?? [];
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryClient.GetCategoryByIdAsync(id);
        return category;
    }

    public async Task AddCategoryAsync(CategoryDto category)
    {
        var responseMessage = await _categoryClient.AddCategoryAsync(category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategoryAsync(CategoryDto category)
    {
        var responseMessage = await _categoryClient.UpdateCategoryAsync(category.Id, category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var responseMessage = await _categoryClient.DeleteCategoryAsync(id);
        responseMessage.EnsureSuccessStatusCode();
    }
}
