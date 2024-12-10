using StudyMart.Web.ViewModels;

namespace StudyMart.Web.Services;

public class CategoryService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    private const string BaseUrl = "/api/categories";

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<Category>>(BaseUrl);
        return categories ?? [];
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        var category = await _httpClient.GetFromJsonAsync<Category>($"{BaseUrl}/{id}");
        return category ?? new Category();
    }

    public async Task AddCategoryAsync(Category category)
    {
        var responseMessage= await _httpClient.PostAsJsonAsync(BaseUrl, category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{category.Id}", category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var responseMessage = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        responseMessage.EnsureSuccessStatusCode();
    }
}
