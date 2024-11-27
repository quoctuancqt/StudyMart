using System.ComponentModel.DataAnnotations;
using StudyMart.ApiService.Data.Common;
using StudyMart.Contract.Category;

namespace StudyMart.ApiService.Data.Entities;

public class Category : ISoftDelete
{
    [Key]
    public int CategoryID { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Product>? Products { get; set; }
    
    public CategoryDto ToDto() => new(CategoryID, Name);
}
