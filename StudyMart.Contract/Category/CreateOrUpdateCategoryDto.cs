using System.ComponentModel.DataAnnotations;

namespace StudyMart.Contract.Category;

public class CreateOrUpdateCategoryDto(string name, string description)
{
    [Required] 
    public string Name { get; private set; } = name;
    [Required]
    public string Description { get; private set; } = description;
}