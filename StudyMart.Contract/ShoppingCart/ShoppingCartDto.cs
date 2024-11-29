namespace StudyMart.Contract.ShoppingCart;

public record ShoppingCartDto(int Id, IList<CartItemDto> CartItems);

public  record CartItemDto(int Quantity, string Name, decimal Price);