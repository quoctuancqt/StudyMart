using System.ComponentModel.DataAnnotations;
using StudyMart.ApiService.Data.Common;
using StudyMart.ApiService.Swagger;
using StudyMart.Contract.Category;

namespace StudyMart.ApiService.Data.Entities;

[SwaggerExclude]
public class Category : ISoftDeletable
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(256)]
    public required string Description { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Product>? Products { get; set; }
    
    public CategoryDto ToDto() => new(CategoryId, Name, Description);
}
