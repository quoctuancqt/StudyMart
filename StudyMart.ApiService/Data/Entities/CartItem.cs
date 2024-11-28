using System.ComponentModel.DataAnnotations;

namespace StudyMart.ApiService.Data.Entities;

public class CartItem
{
    [Key]
    public int CartItemId { get; set; }
    [Required]
    public int Quantity { get; set; }

    // Foreign Keys
    public int ShoppingCartId { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}