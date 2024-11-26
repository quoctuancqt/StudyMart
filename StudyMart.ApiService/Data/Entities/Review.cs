using System.ComponentModel.DataAnnotations;

namespace StudyMart.ApiService.Data.Entities;

public class Review
{
    [Key]
    public int ReviewID { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    // Foreign Keys
    public int ProductID { get; set; }
    public Product? Product { get; set; }

    public int UserID { get; set; }
}
