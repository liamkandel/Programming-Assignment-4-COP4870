using ShopAppLib.Maui.ViewModels;

namespace ShopApp.Maui.Views;

public partial class InventoryView : ContentPage
{

    public InventoryView()
    {
        InitializeComponent();
        BindingContext = new InventoryViewModel();

    }
    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryViewModel).DeleteItem();
    }
    private void EditClicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryViewModel).UpdateItem();
    }
    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AddItem");
    }
    private void BackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
    private void InlineDelete_Clicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryViewModel).RefreshItems();
    }
    private void SpecifyTaxClicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryViewModel).GetTax();

    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as InventoryViewModel).RefreshItems();
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {
        (BindingContext as InventoryViewModel).GetTax();
    }

}