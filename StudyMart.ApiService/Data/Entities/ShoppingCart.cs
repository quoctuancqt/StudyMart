using System.ComponentModel.DataAnnotations;

namespace StudyMart.ApiService.Data.Entities;

public class ShoppingCart
{
    [Key]
    public int CartId { get; set; }

    public string UserId { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }
}
