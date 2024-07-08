// Liam Kandel
//namespace ShopAppLib.Models
//{
//    public class Shop
//    {
//        private List<Item>? cart = new List<Item>(); // Contains items in users shopping cart

//        private Inventory inventory;
//        public Shop(Inventory inv) // Shop must be instantiated with an inventory so that user doesn't worry about changing item stock, all that is done in this class
//        {
//            inventory = inv;
//        }

//        public void PrintCart()
//        {
//            if (cart == null)
//            {
//                Console.WriteLine("Cart is empty.");
//            }
//            else
//            {
//                Console.WriteLine("ID\tItem\tPrice");
//                foreach (var item in cart)
//                {
//                    Console.WriteLine($"{item.Id,-10} {item.Name,-10} {item.Price,-5}");
//                }
//            }
//        }

//        public bool AddToCart(int id)
//        {

//            if (cart == null) { return false; }
//            else if (inventory == null) { return false; }
//            else if (inventory.getItem(id).Stock == 0) { return false; } // If the item has no more available stock
//            else
//            {
//                inventory.getItem(id).Stock--;
//                cart.Add(inventory.getItem(id));
//                return true;
//            }

//        }
//        public bool RemoveFromCart(int id)
//        {
//            if (cart == null) { return false; }
//            else if (inventory == null) { return false; }
//            else if (cart.Count == 0) { return false; }
//            var item = cart.FirstOrDefault(i => i.Id == id);
//            if (item == null) { return false; }
//            else
//            {
//                inventory.getItem(id).Stock++;

//                cart.Remove(item);
//                return true;
//            }

//        }
//        public void CheckOut()
//        {
//            if (cart == null || cart.Count == 0)
//            {
//                Console.WriteLine("Cart is empty.");
//                return;
//            }
//            else
//            {
//                Console.Clear();
//                double subTotal = cart.Sum(x => x.Price); // Sum up all the prices of the item in the cart

//                Console.WriteLine("Name\t\tPrice");
//                foreach (var item in cart)
//                {
//                    Console.WriteLine($"{item.Name,-16}${item.Price}");
//                }
//                Console.WriteLine("--------");
//                Console.WriteLine($"Sub Total: ${Math.Round(subTotal, 2)}");
//                Console.WriteLine($"Tax: ${Math.Round(subTotal * 0.07, 2)}");
//                Console.WriteLine($"Total (with tax): ${Math.Round(subTotal + (subTotal * 0.07), 2)}");

//                cart.Clear();
//                Console.ReadKey();
//            }

//        }
//    }
//}
