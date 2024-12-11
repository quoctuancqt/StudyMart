namespace StudyMart.Web.States;

public class CartContainer
{
    public event Action? OnChange;
    public int TotalItems = 0;

    public void AddToCart()
    {
        TotalItems += 1;
        NotifyStateChanged();
    }
    
    public void ClearCart()
    {
        TotalItems = 0;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}