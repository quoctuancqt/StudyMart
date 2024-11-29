using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyMart.ApiService.Swagger;
using StudyMart.Contract.Order;

namespace StudyMart.ApiService.Data.Entities;

[SwaggerExclude]
public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending; 

    public string UserId { get; set; }

    // Navigation Property
    public ICollection<OrderItem>? OrderItems { get; set; }

    public OrderDto ToDto() => new(OrderId, OrderDate, TotalAmount, Status.ToString());
}
