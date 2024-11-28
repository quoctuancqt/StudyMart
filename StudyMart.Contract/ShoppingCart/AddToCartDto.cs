using System.ComponentModel.DataAnnotations;

namespace StudyMart.Contract.ShoppingCart;

public class AddToCartDto
{
    [Required] 
    public int ProductId { get; set; }
    [Required]
    public int  Quantity { get; set; }
}