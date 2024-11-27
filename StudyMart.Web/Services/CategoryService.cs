using StudyMart.Web.ViewModels;

namespace StudyMart.Web.Services;

public class CategoryService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");

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
        var responseMessage= await _httpClient.PostAsJsonAsync("categories", category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategoryAsync(CategoryModel category)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"categories/{category.CategoryID}", category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var responseMessage = await _httpClient.DeleteAsync($"categories/{id}");
        responseMessage.EnsureSuccessStatusCode();
    }
}
