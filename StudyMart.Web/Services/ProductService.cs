using StudyMart.Contract.Clients;
using StudyMart.Contract.Product;

namespace StudyMart.Web.Services;

public class ProductService(ApiClient apiClient)
{
    private readonly IProductClient _productClient = apiClient.ProductClient;

    public async Task<List<ProductDto>> GetProductsAsync() => await _productClient.GetProductsAsync();

    public async Task<ProductDto?> GetProductByIdAsync(int id) => await _productClient.GetProductByIdAsync(id);

    public async Task AddProductAsync(CreateOrUpdateProductDto product)
    {
        var responseMessage = await _productClient.AddProductAsync(product);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task UpdateProductAsync(int id, CreateOrUpdateProductDto product)
    {
        var responseMessage = await _productClient.UpdateProductAsync(id, product);
        responseMessage.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        var responseMessage = await _productClient.DeleteProductAsync(id);
        responseMessage.EnsureSuccessStatusCode();
    }
}
