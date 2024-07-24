﻿using ShopApp.Maui.DTO;
using ShopAppLib;
using ShopAppLib.Maui.ViewModels;
using ShopAppLib.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShopApp.Maui.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public ShopViewModel()
        {
            InventoryQuery = string.Empty;
            Carts = new ObservableCollection<ShoppingCart>(CartServiceProxy.Current.Carts);
            SelectedCart = Carts.FirstOrDefault();

        }

        private string inventoryQuery;
        public string InventoryQuery
        {
            set
            {
                inventoryQuery = value;
                NotifyPropertyChanged();
            }
            get { return inventoryQuery; }
        }
        public List<ItemViewModel> Items
        {
            get
            {
                return InventoryServiceProxy.Current.Items.Where(p => p != null)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p => new ItemViewModel(p)).ToList()
                    ?? new List<ItemViewModel>();
            }
        }
        public ItemViewModel ItemToBuy { get; set; }

        private ObservableCollection<ShoppingCart> carts;
        public ObservableCollection<ShoppingCart> Carts
        {
            get
            {
                return carts;
            }
            set
            {
                if (carts != value)
                {
                    carts = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ShoppingCart? selectedCart;

        public ShoppingCart? SelectedCart
        {
            get
            {
                return selectedCart;
            }

            set
            {
                if (selectedCart != value)
                {
                    selectedCart = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(SelectedCartItems));
                    NotifyPropertyChanged(nameof(FormattedTax));
                    NotifyPropertyChanged(nameof(FormattedSubTotal));
                    NotifyPropertyChanged(nameof(FormattedTotal));
                }
            }
        }
        public List<ItemDTO> SelectedCartItems
        {
            get
            {
                return SelectedCart?.Contents?.Where(p => p != null)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p => new ItemDTO(p)).ToList()
                    ?? new List<ItemDTO>();
            }
        }

        public decimal SubTotal
        {
            get
            {
                if (SelectedCart?.Contents == null)
                {
                    return 0M;
                }

                return SelectedCart.Contents.Sum(c =>
                {
                    if (c.IsBogo)
                    {
                        int effectiveStock = (c.Stock / 2) + (c.Stock % 2);
                        return c.Price * effectiveStock;
                    }
                    else
                    {
                        return c.Price * c.Stock;
                    }
                });
            }
        }

        public string FormattedSubTotal
        {
            get { return SubTotal.ToString("C"); }
        }

        private decimal taxRate = InventoryServiceProxy.Current.TaxRate;
        public string FormattedTax
        {
            get
            {
                taxRate = InventoryServiceProxy.Current.TaxRate;
                return (taxRate / 100 * SubTotal).ToString("C");
            }
        }

        private decimal Total
        {
            get
            {
                return SubTotal + (taxRate / 100 * SubTotal);
            }
        }

        public string FormattedTotal
        { get { return Total.ToString("C"); } }

        public void Refresh()
        {
            InventoryQuery = string.Empty;
            NotifyPropertyChanged(nameof(Items));
            NotifyPropertyChanged(nameof(ItemToBuy));
            NotifyPropertyChanged(nameof(SelectedCart));
            NotifyPropertyChanged(nameof(SelectedCartItems));
            NotifyPropertyChanged(nameof(Carts));
            NotifyPropertyChanged(nameof(SubTotal));
            NotifyPropertyChanged(nameof(FormattedSubTotal));
            NotifyPropertyChanged(nameof(taxRate));
            NotifyPropertyChanged(nameof(FormattedTax));
            NotifyPropertyChanged(nameof(FormattedTotal));
        }

        public void Search()
        {
            NotifyPropertyChanged(nameof(Items));
        }

        public void PlaceInCart()
        {
            if (ItemToBuy == null) { return; }
            if (ItemToBuy.Stock > 0 && SelectedCart != null)
            {
                CartServiceProxy.Current.AddOrUpdate(ItemToBuy.Item, SelectedCart);
                ReduceItemToBuy();
                NotifyPropertyChanged(nameof(FormattedSubTotal));
                NotifyPropertyChanged(nameof(FormattedTax));
                NotifyPropertyChanged(nameof(FormattedTotal));
                NotifyPropertyChanged(nameof(SelectedCartItems));
            }
        }
        public void ReduceItemToBuy()
        {
            var existingItem = InventoryServiceProxy.Current.Items.FirstOrDefault(i => i.Id == ItemToBuy.Id);
            existingItem.Stock--;
            ItemToBuy.Stock--;

        }

        public void AddNewCart()
        {
            CartServiceProxy.Current.AddNewCart();
            Carts.Clear();
            foreach (var cart in CartServiceProxy.Current.Carts)
            {
                Carts.Add(cart);
            }
            SelectedCart = Carts.LastOrDefault();

        }

        public void ChangeCart()
        {
            Refresh();
        }

        public void Checkout()
        {
            if (SelectedCart == null) { return; }
            else
            {
                CartServiceProxy.Current.RemoveCart(SelectedCart);
            }
            SelectedCart = null;
            NotifyPropertyChanged(nameof(Carts));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

