using StudyMart.Contract.Clients;
using StudyMart.Contract.ShoppingCart;

namespace StudyMart.Web.Services;

public class CartService(ApiClient apiClient)
{
    private readonly ICartClient _cartClient = apiClient.CartClient;

    public async Task<ShoppingCartDto?> GetCartAsync()
    {
        var cart = await _cartClient.GetCartAsync();
        return cart;
    }

    public async Task<bool> AddToCartAsync(IEnumerable<AddToCartDto> cartItems)
    {
        var response = await _cartClient.AddToCartAsync(cartItems);
        return response.IsSuccessStatusCode;
    }
}