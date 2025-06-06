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

    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [Required] public string Address { get; set; } = string.Empty;

    public string Address2 { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<OrderItem>? OrderItems { get; set; }

    public OrderDto ToDto() => new(OrderId, OrderDate, TotalAmount, Status.ToString());
}
