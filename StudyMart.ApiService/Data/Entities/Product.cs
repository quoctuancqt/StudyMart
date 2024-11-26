using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyMart.ApiService.Data.Entities;

public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string ImageURL { get; set; } = string.Empty;

    // Foreign Key
    public int CategoryID { get; set; }
    public Category? Category { get; set; }

    // Navigation Properties
    public ICollection<OrderItem>? OrderItems { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<ShoppingCart>? ShoppingCarts { get; set; }
}
