using StudyMart.Web.ViewModels;

namespace StudyMart.Web.States.Cart;

public class CartStateContainer
{
    private List<CartItem>? _items;

    public List<CartItem> Items
    {
        get => _items ?? [];
        set
        {
            _items = value;
            NotifyStateChanged();
        }
    }
    
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}