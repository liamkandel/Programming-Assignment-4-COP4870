using ShopApp.Maui.DTO;
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


        public string query { get; set; }

        public List<ItemViewModel> Items
        {
            get
            {
                InventoryServiceProxy.Current.Get();
                return (InventoryServiceProxy.Current?.Items?.Select(c => new ItemViewModel(c)).ToList()
                    ?? new List<ItemViewModel>());

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

        public async void RefreshItems()
        {
            await InventoryServiceProxy.Current.Get();
            NotifyPropertyChanged(nameof(Items));
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

        public async void DeleteItem()
        {
            if (SelectedItem?.Item == null)
            {
                return;
            }

            await InventoryServiceProxy.Current.Delete(SelectedItem.Item.Id);
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

        public async void Search()
        {
            if (query == null) return;
            await InventoryServiceProxy.Current.Search(new Utilities.Query { QueryString = query });
            NotifyPropertyChanged(nameof(Items));
        }


        public async Task<IEnumerable<ItemDTO>> ImportCSV()
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a CSV file",
            });

            if (fileResult != null)
            {
                using var stream = await fileResult.OpenReadAsync();
                using var reader = new StreamReader(stream);

                var items = new List<ItemDTO>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var columns = line.Split(',');

                    if (columns.Length >= 8)
                    {
                        var item = new ItemDTO
                        {
                            Name = columns[0],
                            Description = columns[1],
                            Price = decimal.Parse(columns[2]),
                            Stock = int.Parse(columns[3]),
                            IsBogo = bool.Parse(columns[4]),
                            IsMarkedDown = bool.Parse(columns[5]),
                            MarkedDownPrice = decimal.Parse(columns[6]),
                            MarkDown = decimal.Parse(columns[7])
                        };
                        items.Add(item);
                        await InventoryServiceProxy.Current.AddOrUpdate(item);
                        RefreshItems();
                    }
                    else { break; }
                }

                return items;
            }

            return null;
        }
    }
}
