using System.ComponentModel.DataAnnotations;

namespace StudyMart.Contract.Order;

public class CreateOrderDto
{
    [Required] public string FirstName { get; set; } = string.Empty;

    [Required] public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    [Required] public string Address { get; set; } = string.Empty;

    public string Address2 { get; set; } = string.Empty;
}