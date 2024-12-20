using Refit;
using StudyMart.Contract.Product;

namespace StudyMart.Contract.Clients;

public interface IProductClient
{
    [Get("/api/v1/products")]
    Task<List<ProductDto>> GetProductsAsync();
    
    [Get("/api/v1/products/{id}")]
    Task<ProductDto?> GetProductByIdAsync(int id);
    
    [Post("/api/v1/products")]
    Task<HttpResponseMessage> AddProductAsync(CreateOrUpdateProductDto product);
    
    [Put("/api/v1/products/{id}")]
    Task<HttpResponseMessage> UpdateProductAsync(int id, CreateOrUpdateProductDto product);
    
    [Delete("/api/v1/products/{id}")]
    Task<HttpResponseMessage> DeleteProductAsync(int id);
    
    [Post("/api/v1/products/{id}/reviews")]
    Task<HttpResponseMessage> AddReviewAsync(int id, CreateReviewDto review);
}