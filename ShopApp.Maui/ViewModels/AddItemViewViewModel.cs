namespace ShopApp.Maui.ViewModels
{
    class AddItemViewViewModel
    {
        private string name;
        public string _Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        private string description;
        public string _Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }

        private string price;
        public string _Price
        {
            get { return price; }
            set
            {
                description = value;
            }
        }

        private string stock;
        public string _Stock
        {
            get { return stock; }
            set
            {
                stock = value;
            }
        }
    }
}
