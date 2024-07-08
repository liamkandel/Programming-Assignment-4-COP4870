using ShopAppLib.Models;

namespace ShopAppLib
{
    public class CartServiceProxy
    {

        private CartServiceProxy()
        {
            carts.Add(new ShoppingCart { Id = 1, Contents = new List<Item>() });
        }

        private static CartServiceProxy? instance;
        private static object instanceLock = new object();

        public int LastId
        {
            get
            {
                if (carts?.Any() ?? false)
                {
                    return carts?.Select(c => c.Id)?.Max() + 1 ?? 0;
                }
                return 0;
            }
        }
        public static CartServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CartServiceProxy();
                    }
                }

                return instance;
            }
        }

        private List<ShoppingCart> carts = new List<ShoppingCart>();
        public List<ShoppingCart> Carts
        {
            get { return carts; }
        }

        public void AddNewCart()
        {
            carts.Add(new ShoppingCart
            {
                Id = LastId
            }
            );
        }

        public Item? AddOrUpdate(Item? i, ShoppingCart c)
        {
            if (carts == null) { return null; }
            if (c == null) { return null; }

            var isAdd = true;

            var existingCart = carts.FirstOrDefault(C => C.Id == c.Id);

            if (existingCart == null) { return null; }

            var existingItem = existingCart.Contents.FirstOrDefault(c => c.Id == i.Id);


            if (existingItem != null)
            {
                if (existingItem.IsBogo == i.IsBogo)
                    existingItem.Stock++;
                existingItem.Name = i.Name;
                existingItem.Description = i.Description;
                existingItem.Id = i.Id;
                existingItem.Price = i.Price;
                existingItem.IsBogo = i.IsBogo;
                existingItem.IsMarkedDown = i.IsMarkedDown;
                existingItem.MarkDown = i.MarkDown;
                existingItem.MarkedDownPrice = i.MarkedDownPrice;
                if (i.IsMarkedDown)
                {
                    existingItem.Price = i.MarkedDownPrice;
                }
            }
            else
            {
                Item newItem = new Item
                {
                    Name = i.Name,
                    Price = i.Price,
                    Id = i.Id,
                    Stock = 1,
                    IsBogo = i.IsBogo,
                    IsMarkedDown = i.IsMarkedDown,
                    MarkedDownPrice = i.MarkedDownPrice,
                    MarkDown = i.MarkDown
                };

                if (i.IsMarkedDown)
                { newItem.Price = i.MarkedDownPrice; }

                existingCart.Contents.Add(newItem);
            }


            return i;
        }

        public Item? UpdateCarts(Item i)
        {
            {
                if (i == null) { return null; }

                foreach (var cart in carts)
                {
                    var existingItem = cart.Contents.FirstOrDefault(item => item.Id == i.Id);

                    if (existingItem == null)
                    {
                    }
                    else
                    {
                        Item newItem = new Item
                        {
                            Name = i.Name,
                            Price = i.Price,
                            Id = i.Id,
                            Stock = 1,
                            IsBogo = i.IsBogo,
                            IsMarkedDown = i.IsMarkedDown,
                            MarkedDownPrice = i.MarkedDownPrice,
                            MarkDown = i.MarkDown
                        };

                        if (i.IsMarkedDown)
                        {
                            newItem.Price = i.MarkedDownPrice;
                        }
                        AddOrUpdate(newItem, cart);
                    }
                }

                return i;
            }
        }

        public void Delete(int id)
        {
            {

                foreach (var cart in carts)
                {
                    var existingItem = cart.Contents.FirstOrDefault(item => item.Id == id);

                    if (existingItem == null)
                    {
                    }
                    else
                    {
                        cart.Contents.Remove(existingItem);
                    }
                }


            }
        }

        public void RemoveCart(ShoppingCart c)
        {
            var existingCart = Carts.FirstOrDefault(cart => cart.Id == c.Id);
            if (existingCart == null) { return; }
            else
            { Carts.Remove(existingCart); }
        }
    }
}