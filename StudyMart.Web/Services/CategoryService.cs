using StudyMart.Web.ViewModels;

namespace StudyMart.Web.Services;

public class CategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    }

    public async Task<List<CategoryModel>> GetCategoriesAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryModel>>("categories");
        return categories ?? [];
    }

    public async Task<CategoryModel> GetCategoryByIdAsync(int id)
    {
        var category = await _httpClient.GetFromJsonAsync<CategoryModel>($"categories/{id}");
        return category ?? new CategoryModel();
    }

    public async Task AddCategoryAsync(CategoryModel category)
    {
        _ = await _httpClient.PostAsJsonAsync("categories", category);
    }

    public async Task UpdateCategoryAsync(CategoryModel category)
    {
        _ = await _httpClient.PutAsJsonAsync($"categories/{category.Id}", category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        _ = await _httpClient.DeleteAsync($"categories/{id}");
    }
}
