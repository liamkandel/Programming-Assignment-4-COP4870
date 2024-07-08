using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShopAppLib.Maui.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<ItemViewModel> Items
        {
            get
            {
                return InventoryServiceProxy.Current?.Items?.Select(c => new ItemViewModel(c)).ToList()
                    ?? new List<ItemViewModel>();
            }
        }



        public ItemViewModel SelectedItem { get; set; }

        private bool entryIsVisible;
        public bool EntryIsVisible
        {
            get { return entryIsVisible; }
            set
            {
                entryIsVisible = value;
                NotifyPropertyChanged(nameof(EntryIsVisible));
            }
        }

        public bool taxWarningIsVisible;
        public bool TaxWarningIsVisible
        {
            get { return taxWarningIsVisible; }
            set
            {
                taxWarningIsVisible = value;
                NotifyPropertyChanged(nameof(TaxWarningIsVisible));
            }
        }

        public string taxRate = InventoryServiceProxy.Current.TaxRate.ToString();
        public string? TaxRate
        {
            get => taxRate;
            set { taxRate = value ?? string.Empty; }
        }

        public InventoryViewModel()
        {
            EntryIsVisible = false;
            TaxWarningIsVisible = false;
        }

        public void RefreshItems()
        {
            NotifyPropertyChanged("Items");
        }

        public void UpdateItem()
        {
            if (SelectedItem?.Item == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//AddItem?itemId={SelectedItem.Item.Id}");
            //$"//ProjectDetail?clientId={Model.ClientId}"
            InventoryServiceProxy.Current.AddOrUpdate(SelectedItem.Item);
        }

        public void DeleteItem()
        {
            if (SelectedItem?.Item == null)
            {
                return;
            }

            InventoryServiceProxy.Current.Delete(SelectedItem.Item.Id);
            CartServiceProxy.Current.Delete(SelectedItem.Item.Id);
            RefreshItems();
        }

        public void GetTax()
        {

            if (decimal.TryParse(TaxRate, out var val))
            {
                InventoryServiceProxy.Current.TaxRate = val;
                TaxWarningIsVisible = false;
            }
            else
            {
                InventoryServiceProxy.Current.TaxRate = 0;
                TaxWarningIsVisible = true;
            }

            if (!EntryIsVisible || TaxWarningIsVisible) { EntryIsVisible = true; }
            else { EntryIsVisible = false; }
            NotifyPropertyChanged(nameof(EntryIsVisible));
            NotifyPropertyChanged(nameof(TaxWarningIsVisible));
            NotifyPropertyChanged(nameof(TaxRate));
        }

    }
}
