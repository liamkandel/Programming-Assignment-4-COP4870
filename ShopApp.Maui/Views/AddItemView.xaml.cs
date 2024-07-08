using ShopAppLib.Maui.ViewModels;

namespace ShopApp.Maui.Views;

[QueryProperty(nameof(itemId), "itemId")]
public partial class AddItemView : ContentPage
{
    public int itemId { get; set; }
    public AddItemView()
    {
        InitializeComponent();
    }


    private void OnGoBack(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Inventory");
    }

    private void onAddNewItem(object sender, EventArgs e)
    {
        (BindingContext as ItemViewModel).Add();

        Shell.Current.GoToAsync("//Inventory");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ItemViewModel(itemId);
    }
}