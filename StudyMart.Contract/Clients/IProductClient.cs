using Refit;
using StudyMart.Contract.Product;

namespace StudyMart.Contract.Clients;

public interface IProductClient
{
    [Get("/api/products")]
    Task<List<ProductDto>> GetProductsAsync();
    
    [Get("/api/products/{id}")]
    Task<ProductDto?> GetProductByIdAsync(int id);
    
    [Post("/api/products")]
    Task<HttpResponseMessage> AddProductAsync(CreateOrUpdateProductDto product);
    
    [Put("/api/products/{id}")]
    Task<HttpResponseMessage> UpdateProductAsync(int id, CreateOrUpdateProductDto product);
    
    [Delete("/api/products/{id}")]
    Task<HttpResponseMessage> DeleteProductAsync(int id);
    
    [Post("/api/products/{id}/reviews")]
    Task<HttpResponseMessage> AddReviewAsync(int id, CreateReviewDto review);
}