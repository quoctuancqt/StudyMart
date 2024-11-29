using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudyMart.ApiService.Authorization;
using StudyMart.ApiService.Data;
using StudyMart.Contract.ShoppingCart;

namespace StudyMart.ApiService.Features.ShoppingCart;

internal static class ShoppingCartApi
{
    public static RouteGroupBuilder MapShoppingCartEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/shopping-carts")
            .WithTags("Shopping Cart");

        group.RequireAuthorization();

        group.MapGet("/", async Task<Results<Ok<ShoppingCartDto>, NotFound>> (AppDbContext db, CurrentUser currentUser) =>
        {
            var cart = await db.ShoppingCarts
                .Include(sc => sc.CartItems)!
                .ThenInclude(ci => ci.Product)
                .Where(sc => sc.UserId == currentUser.UserId)
                .Select(sc => new ShoppingCartDto(sc.CartId,
                    sc.CartItems!.Select(ci => new CartItemDto(ci.Quantity, ci.Product!.Name, ci.Product!.Price))
                        .ToList()))
                .FirstOrDefaultAsync();

            if (cart is null) return TypedResults.NotFound();
            
            return TypedResults.Ok(cart);
        });
        
        group.MapPost("/", async Task<Results<BadRequest<HttpValidationProblemDetails>, NoContent>> (AddToCartDto addToCartDto, AppDbContext db, CurrentUser currentUser) =>
        {
            var cart = await db.ShoppingCarts
                .Include(sc => sc.CartItems)
                .FirstOrDefaultAsync(sc => sc.UserId == currentUser.UserId);

            if (cart is null)
            {
                cart = new Data.Entities.ShoppingCart
                {
                    UserId = currentUser.UserId!,
                    CartItems = new List<Data.Entities.CartItem>()
                };
                db.ShoppingCarts.Add(cart);
            }

            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == addToCartDto.ProductId);
            if (cartItem is null)
            {
                cartItem = new Data.Entities.CartItem
                {
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity
                };
                cart.CartItems!.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += addToCartDto.Quantity;
            }

            await db.SaveChangesAsync();
            
            return TypedResults.NoContent();
        });
        
        group.MapDelete("/{productId}", async Task<Results<NoContent, NotFound>> (int productId, AppDbContext db, CurrentUser currentUser) =>
        {
            var cart = await db.ShoppingCarts
                .Include(sc => sc.CartItems)
                .FirstOrDefaultAsync(sc => sc.UserId == currentUser.UserId);

            if (cart is null)
            {
                return TypedResults.NotFound();
            }

            var cartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem is null)
            {
                return TypedResults.NotFound();
            }

            cart.CartItems!.Remove(cartItem);
            await db.SaveChangesAsync();
            
            return TypedResults.NoContent();
        });

        return group;
    }
}