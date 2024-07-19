using ShopApp.Maui.DTO;

namespace ShopAppLib.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<ItemDTO>? Contents { get; set; }

        public ShoppingCart()
        {
            Contents = new List<ItemDTO>();
        }
    }
}
