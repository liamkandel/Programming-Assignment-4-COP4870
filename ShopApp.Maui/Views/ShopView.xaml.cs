using ShopApp.Maui.ViewModels;

namespace ShopApp.Maui.Views;

public partial class ShopView : ContentPage
{
    public ShopView()
    {
        InitializeComponent();
        BindingContext = new ShopViewModel();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel).Refresh();
    }

    private void InventorySearchClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).Search();
    }


    private void ContentPage_NavigatedTo_1(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel).Refresh();
    }

    private void AddNewCart(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).AddNewCart();
    }

    private void ChangeCart(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).ChangeCart();
    }

    private void PlaceInCart(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).PlaceInCart();
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).Checkout();
    }
}