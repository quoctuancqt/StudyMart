using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StudyMart.ApiService.Authorization;
using StudyMart.ApiService.Data;
using StudyMart.Contract.Order;

namespace StudyMart.ApiService.Features.Order;

internal static class OrderApi
{
    public static RouteGroupBuilder MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/orders")
            .WithTags("Order");

        group.RequireAuthorization();
        
        group.MapGet("/{id}", async Task<Results<Ok<OrderDto>, NotFound>> (AppDbContext db, int id) =>
        {
            return await db.Orders.FindAsync(id) switch
            {
                { } order => TypedResults.Ok(order.ToDto()),
                _ => TypedResults.NotFound()
            };
        });
        
        group.MapDelete("{id}", async Task<Results<NotFound, NoContent>> (AppDbContext db, int id) =>
        {
            var order = await db.Orders.FindAsync(id);
            if (order is null) return TypedResults.NotFound();

            order.Status = OrderStatus.Cancelled;
            db.Orders.Update(order);
            await db.SaveChangesAsync();
            
            return TypedResults.NoContent();
        });
        
        group.MapPost("/", async Task<Results<Created<OrderDto>, NotFound, BadRequest<HttpValidationProblemDetails>>> (CurrentUser currentUser, AppDbContext db) =>
        {
            var cart = await db.ShoppingCarts
                .Include(sc => sc.CartItems)!
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(sc => sc.UserId == currentUser.UserId);

            if (cart is null) return TypedResults.NotFound();
            
            var order = new Data.Entities.Order
            {
                UserId = currentUser.UserId!,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems!.Sum(ci => ci.Product!.Price * ci.Quantity),
            };
            db.Orders.Add(order);

            await db.SaveChangesAsync();
            
            foreach (var cartItem in cart.CartItems!)
            {
                var orderItem = new Data.Entities.OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product!.Price
                };
                db.OrderItems.Add(orderItem);
            }

            await db.SaveChangesAsync();
            
            return TypedResults.Created($"/api/orders/{order.OrderId}", order.ToDto());
        });

        return group;
    }
}