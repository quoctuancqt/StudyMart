using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OpenTelemetry.Trace;
using StudyMart.ApiService.Authorization;
using StudyMart.ApiService.Data;
using StudyMart.ApiService.Filters;
using StudyMart.Contract.Order;
using StudyMart.MailKit.Client;

namespace StudyMart.ApiService.Features.Order;

internal static class OrderApi
{
    public static RouteGroupBuilder MapOrderEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/orders")
            .WithTags("Order");

        group.RequireAuthorization();

        group.WithParameterValidation(typeof(CreateOrderDto));
        
        group.MapGet("/", async Task<Results<Ok<IEnumerable<OrderDto>>, NotFound>> (AppDbContext db, CurrentUser currentUser) =>
        {
            var orders = await db.Orders
                .AsNoTracking()
                .Where(o => o.UserId == currentUser.UserId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return TypedResults.Ok(orders.Select(o => o.ToDto()));
        });
        
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
            var effected = await db.Orders.Where(o => o.OrderId == id).ExecuteDeleteAsync();
            return effected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        });
        
        group.MapPost("/", async Task<Results<Created<OrderDto>, NotFound, BadRequest<HttpValidationProblemDetails>>> ([FromBody] CreateOrderDto dto, CurrentUser currentUser, AppDbContext db) =>
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
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Address2 = dto.Address2
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
        
        group.MapPut("/{id}/status/{status}", async Task<Results<NoContent, NotFound>> (int id, OrderStatus status, AppDbContext db, MailKitClientFactory factory) =>
        {
            var effected = await db.Orders.Where(o => o.OrderId == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(o => o.Status, status));

            if (effected == 0) return TypedResults.NotFound();
            
            ISmtpClient client = await factory.GetSmtpClientAsync();

            var order = await db.Orders.FindAsync(id);

            if (string.IsNullOrEmpty(order!.Email)) 
                return TypedResults.NoContent();
            
            using var message = new MailMessage("newsletter@yourcompany.com", order.Email);
            message.Subject = $"Your order has been change status to {status.ToString()}!";
            message.Body = "Thank you for ordering!";

            await client.SendAsync(MimeMessage.CreateFromMailMessage(message));

            return TypedResults.NoContent();
        });

        return group;
    }
}