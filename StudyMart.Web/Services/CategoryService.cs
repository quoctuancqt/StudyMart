using StudyMart.Contract.Category;

namespace StudyMart.Web.Services;

public class CategoryService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    private const string BaseUrl = "/api/categories";

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(BaseUrl);
        return categories ?? [];
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _httpClient.GetFromJsonAsync<CategoryDto>($"{BaseUrl}/{id}");
        return category;
    }

    public async Task AddCategoryAsync(CategoryDto category)
    {
        var responseMessage= await _httpClient.PostAsJsonAsync(BaseUrl, category);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateCategoryAsync(CategoryDto category)
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
