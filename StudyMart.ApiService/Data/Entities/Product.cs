using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyMart.ApiService.Data.Common;
using StudyMart.ApiService.Swagger;
using StudyMart.Contract.Product;

namespace StudyMart.ApiService.Data.Entities;

[SwaggerExclude]
public class Product : ISoftDeletable
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(250)]
    public required string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [MaxLength(250)]
    public string ImageUrl { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; }

    // Foreign Key
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<CartItem>? CartItems { get; set; }

    public ProductDto ToDto() => new(ProductId, Name, Description, Price, ImageUrl, Category!.Name);
}
