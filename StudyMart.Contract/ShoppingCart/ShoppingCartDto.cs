namespace StudyMart.Contract.ShoppingCart;

public class ShoppingCartDto
{
    public int Id { get; set; }
    public List<CartItemDto>? CartItems { get; set; }
}

public class CartItemDto
{
    public int Quantity { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int ProductId { get; set; }
}