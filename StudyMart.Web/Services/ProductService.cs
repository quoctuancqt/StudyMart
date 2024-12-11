using StudyMart.Contract.Product;

namespace StudyMart.Web.Services;

public class ProductService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    private const string BaseUrl = "/api/products";

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>(BaseUrl);
        return products ?? [];
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _httpClient.GetFromJsonAsync<ProductDto>($"{BaseUrl}/{id}");
        return product;
    }

    public async Task AddProductAsync(ProductDto product)
    {
        var responseMessage= await _httpClient.PostAsJsonAsync(BaseUrl, product);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateProductAsync(ProductDto product)
    {
        var responseMessage = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{product.Id}", product);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        var responseMessage = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        responseMessage.EnsureSuccessStatusCode();
    }
}
