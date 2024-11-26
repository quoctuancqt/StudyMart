using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyMart.ApiService.Data.Entities;

public class OrderItem
{
    [Key]
    public int OrderItemID { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    // Foreign Keys
    public int OrderID { get; set; }
    public Order? Order { get; set; }

    public int ProductID { get; set; }
    public Product? Product { get; set; }
}
