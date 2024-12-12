using System.Text;
using System.Text.Json;
using StudyMart.Contract.Order;

namespace StudyMart.Web.Services;

public class OrderService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    private const string BaseUrl = "/api/orders";
    
    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        var orders = await _httpClient.GetFromJsonAsync<IEnumerable<OrderDto>>(BaseUrl);
        return orders ?? [];
    }
    
    public async Task<OrderDto?> GetOrderAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        if (!response.IsSuccessStatusCode) return null;
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    
    public async Task<bool> CancelOrderAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
    
    public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
        if (!response.IsSuccessStatusCode) return null;
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}