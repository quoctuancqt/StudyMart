using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyMart.ApiService.Data.Entities;

public class Order
{
    [Key]
    public int OrderID { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending; 

    public int UserID { get; set; }

    // Navigation Property
    public ICollection<OrderItem>? OrderItems { get; set; }
}
