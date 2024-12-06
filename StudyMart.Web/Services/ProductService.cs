using System;
using StudyMart.Web.ViewModels;

namespace StudyMart.Web.Services;

public class ProductService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    private const string BaseUrl = "/api/products";

    public async Task<List<Product>> GetProductsAsync()
    {
        var products = await _httpClient.GetFromJsonAsync<List<Product>>(BaseUrl);
        return products ?? [];
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product = await _httpClient.GetFromJsonAsync<Product>($"{BaseUrl}/{id}");
        return product ?? new Product();
    }

    public async Task AddProductAsync(Product product)
    {
        var responseMessage= await _httpClient.PostAsJsonAsync(BaseUrl, product);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateProductAsync(Product product)
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
