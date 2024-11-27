using System.ComponentModel.DataAnnotations;

namespace StudyMart.Web.ViewModels;

public class CategoryModel
{
    public int CategoryID { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}
