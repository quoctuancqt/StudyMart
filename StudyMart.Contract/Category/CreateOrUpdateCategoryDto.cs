using System.ComponentModel.DataAnnotations;

namespace StudyMart.Contract.Category;

public class CreateOrUpdateCategoryDto(string name)
{
    [Required] 
    public string Name { get; private set; } = name;
}