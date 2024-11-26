using System.ComponentModel.DataAnnotations;

namespace StudyMart.ApiService.Data.Entities;

public class ShoppingCart
{
    [Key]
    public int CartID { get; set; }

    [Required]
    public int Quantity { get; set; }

    public int UserID { get; set; }

    public int ProductID { get; set; }
    public Product? Product { get; set; }
}
