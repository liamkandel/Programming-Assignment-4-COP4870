using ShopApp.Maui.DTO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace ShopAppLib.Maui.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand? EditCommand { get; private set; }
        public ICommand? DeleteCommand { get; private set; }

        public ICommand? BOGOCommand { get; private set; }

        public ItemDTO? Item;

        private string? name;
        public string? Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string? description;
        public string? Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private decimal price;
        public decimal Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int stock;
        public int Stock
        {
            get => stock;
            set
            {
                if (stock != value)
                {
                    stock = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool isBogo;
        public bool IsBogo
        {
            get => isBogo;
            set
            {
                if (isBogo != value)
                {
                    isBogo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private decimal markDown;
        public decimal MarkDown
        {
            get { return markDown; }
            set
            {
                if (markDown != value)
                {
                    markDown = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private decimal markedDownPrice;
        public decimal MarkedDownPrice
        {
            get { return markedDownPrice; }
            set
            {

                if (markedDownPrice != value)
                {
                    markedDownPrice = value;
                    NotifyPropertyChanged();
                }
            }

        }

        public void ChangeMarkedDownPrice()
        {
            markedDownPrice = Price - (Price * (MarkDown / 100));
            NotifyPropertyChanged(nameof(MarkedDownPrice));
        }


        public bool IsMarkedDown { get; set; }
        public string FormattedPrice
        {
            get { return price.ToString("C"); }
        }

        public string FormattedMarkedDown
        {
            get { return markedDownPrice.ToString("C"); }
        }
        public int Id { get; set; }

        private void ExecuteEdit(ItemViewModel? i)
        {
            if (i?.Item == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//AddItem?itemId={i.Item.Id}");
        }

        private void ExecuteDelete(int? id)
        {
            if (id == null)
            {
                return;
            }


            InventoryServiceProxy.Current.Delete(id ?? 0);
            CartServiceProxy.Current.Delete(id ?? 0);
        }

        private void ExecuteBOGO(int? id)
        {
            if (id == null) { return; }


            IsBogo = !IsBogo;
            Add();
        }

        public void Add()
        {
            ChangeMarkedDownPrice();
            if (this.MarkDown == 0) { this.IsMarkedDown = false; }
            else { this.IsMarkedDown = true; }
            var newItem = new ItemDTO
            {
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Stock = this.Stock,
                Id = this.Id,
                IsBogo = this.IsBogo,
                IsMarkedDown = this.IsMarkedDown,
                MarkDown = this.MarkDown,
                MarkedDownPrice = this.MarkedDownPrice,

            };
            InventoryServiceProxy.Current.AddOrUpdate(newItem);
            CartServiceProxy.Current.UpdateCarts(newItem);
        }

        public void SetupCommands()
        {
            EditCommand = new Command(
               (c) => ExecuteEdit(c as ItemViewModel));
            DeleteCommand = new Command(
               (c) => ExecuteDelete((c as ItemViewModel)?.Item?.Id));
            BOGOCommand = new Command(
               (c) => ExecuteBOGO((c as ItemViewModel)?.Item?.Id));
        }

        public ItemViewModel()
        {
            Item = new ItemDTO();
            SetupCommands();
        }

        public ItemViewModel(int id)
        {
            Item = InventoryServiceProxy.Current?.Items?.FirstOrDefault(i => i.Id == id);

            if (Item == null)
            {
                Item = new ItemDTO();
            }

            Id = Item.Id;
            Name = Item.Name;
            Description = Item.Description;
            Price = Item.Price;
            Stock = Item.Stock;
            IsMarkedDown = Item.IsMarkedDown;
            MarkDown = Item.MarkDown;
            MarkedDownPrice = Item.MarkedDownPrice;

        }

        public ItemViewModel(ItemDTO i)
        {
            Item = i;
            Id = Item.Id;
            Name = Item.Name;
            Description = Item.Description;
            Price = Item.Price;
            Stock = Item.Stock;
            IsBogo = Item.IsBogo;
            IsMarkedDown = Item.IsMarkedDown;
            MarkDown = Item.MarkDown;
            MarkedDownPrice = Item.MarkedDownPrice;

            SetupCommands();

        }

        public string? Display
        {
            get
            {
                return ToString();
            }
        }
    }
}
