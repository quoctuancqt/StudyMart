using System.ComponentModel.DataAnnotations;
using StudyMart.ApiService.Swagger;

namespace StudyMart.ApiService.Data.Entities;

[SwaggerExclude]
public class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    // Foreign Keys
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public string UserId { get; set; }
}
