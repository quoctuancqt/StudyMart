using Refit;
using StudyMart.Contract.Order;

namespace StudyMart.Contract.Clients;

public interface IOrderClient
{
    [Get("/api/orders")]
    Task<List<OrderDto>> GetOrdersAsync();
    
    [Get("/api/orders/{id}")]
    Task<OrderDto?> GetOrderByIdAsync(int id);
    
    [Delete("/api/orders/{id}")]
    Task<HttpResponseMessage> DeleteOrderAsync(int id);
    
    [Post("/api/orders")]
    Task<OrderDto> CreateOrderAsync(CreateOrderDto order);
    
    [Put("/api/orders/{id}/status/{status}")]
    Task<HttpResponseMessage> UpdateOrderStatusAsync(int id, OrderStatus status);
}