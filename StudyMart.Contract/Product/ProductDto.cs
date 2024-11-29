namespace StudyMart.Contract.Product;

public record ProductDto(int Id, string Name, string Description, decimal Price, string ImageUrl, string CategoryName);