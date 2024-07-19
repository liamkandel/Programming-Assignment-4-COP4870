using ShopAppLib.Utilities;

namespace ShopApp.Maui
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Console.WriteLine(new WebRequestHandler().Get("/WeatherForecast"));
        }


        private void onInventoryClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Inventory");
        }

        private void onShopClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Shop");
        }
    }

}
