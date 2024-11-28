using System.ComponentModel.DataAnnotations;

namespace StudyMart.Contract.Product;

public class CreateOrUpdateProductDto
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(250)]
    public required string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [MaxLength(250)]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Required]
    public int CategoryId { get; set; }
}