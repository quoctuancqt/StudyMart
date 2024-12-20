using Refit;
using StudyMart.Contract.Order;

namespace StudyMart.Contract.Clients;

public interface IOrderClient
{
    [Get("/api/v1/orders")]
    Task<List<OrderDto>> GetOrdersAsync();
    
    [Get("/api/v1/orders/{id}")]
    Task<OrderDto?> GetOrderByIdAsync(int id);
    
    [Delete("/api/v1/orders/{id}")]
    Task<HttpResponseMessage> DeleteOrderAsync(int id);
    
    [Post("/api/v1/orders")]
    Task<OrderDto> CreateOrderAsync(CreateOrderDto order);
    
    [Put("/api/v1/orders/{id}/status/{status}")]
    Task<HttpResponseMessage> UpdateOrderStatusAsync(int id, OrderStatus status);
}