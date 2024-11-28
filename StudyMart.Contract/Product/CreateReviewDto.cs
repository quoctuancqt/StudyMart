using System.ComponentModel.DataAnnotations;

namespace StudyMart.Contract.Product;

public class CreateReviewDto
{
    [Required]
    [Range(1,5)]
    public int Rating { get; set; }
    [Required]
    public string Comment { get; set; } = string.Empty;
}