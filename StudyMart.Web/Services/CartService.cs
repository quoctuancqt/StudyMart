using StudyMart.Contract.ShoppingCart;

namespace StudyMart.Web.Services;

public class CartService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("StudyMartApi");
    private const string BaseUrl = "/api/shopping-carts";

    public async Task<ShoppingCartDto?> GetCartAsync()
    {
        var cart = await _httpClient.GetFromJsonAsync<ShoppingCartDto>(BaseUrl);
        return cart;
    }

    public async Task<bool> AddToCartAsync(IEnumerable<AddToCartDto> cartItems)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync($"{BaseUrl}/batch", cartItems);
        return responseMessage.IsSuccessStatusCode;
    }
}