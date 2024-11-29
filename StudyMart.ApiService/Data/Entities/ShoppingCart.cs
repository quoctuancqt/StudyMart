using System.ComponentModel.DataAnnotations;
using StudyMart.ApiService.Swagger;

namespace StudyMart.ApiService.Data.Entities;

[SwaggerExclude]
public class ShoppingCart
{
    [Key]
    public int CartId { get; set; }

    public string UserId { get; set; }

    public ICollection<CartItem>? CartItems { get; set; }
}
