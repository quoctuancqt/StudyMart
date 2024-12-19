using Refit;
using StudyMart.Contract.ShoppingCart;

namespace StudyMart.Contract.Clients;

public interface ICartClient
{
    [Get("/api/shopping-carts")]
    Task<ShoppingCartDto?> GetCartAsync();

    [Post("/api/shopping-carts/batch")]
    Task<HttpResponseMessage> AddToCartAsync(IEnumerable<AddToCartDto> cartItems);
}