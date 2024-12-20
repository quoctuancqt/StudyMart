using Refit;
using StudyMart.Contract.ShoppingCart;

namespace StudyMart.Contract.Clients;

public interface ICartClient
{
    [Get("/api/v1/shopping-carts")]
    Task<ShoppingCartDto?> GetCartAsync();

    [Post("/api/v1/shopping-carts/batch")]
    Task<HttpResponseMessage> AddToCartAsync(IEnumerable<AddToCartDto> cartItems);
}