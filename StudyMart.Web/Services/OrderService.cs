using StudyMart.Contract.Clients;
using StudyMart.Contract.Order;

namespace StudyMart.Web.Services;

public class OrderService(ApiClient apiClient)
{
    private readonly IOrderClient _orderClient = apiClient.OrderClient;
    
    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderClient.GetOrdersAsync();
        return orders ?? [];
    }

    public async Task<OrderDto?> GetOrderAsync(int id) => await _orderClient.GetOrderByIdAsync(id);
    
    public async Task<bool> CancelOrderAsync(int id)
    {
        var response = await _orderClient.DeleteOrderAsync(id);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto dto) =>  await _orderClient.CreateOrderAsync(dto);

    public async Task ChangeOrderAynsc(int id, OrderStatus status) => await _orderClient.UpdateOrderStatusAsync(id, status);
}