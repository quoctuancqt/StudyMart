namespace StudyMart.Contract.Order;

public record OrderDto(int OrderId, DateTime OrderDate, decimal TotalAmount, string Status = "Pending");