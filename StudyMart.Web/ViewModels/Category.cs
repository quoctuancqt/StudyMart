using System.ComponentModel.DataAnnotations;

namespace StudyMart.Web.ViewModels;

public class Category
{
    public int CategoryID { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}
