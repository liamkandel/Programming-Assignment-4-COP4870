using ShopAppLib.Models;

namespace ShopApp.Maui.DTO
{
    public class ItemDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set { price = value >= 0 ? value : 0; }
        }

        public string FormattedPrice
        { get { return price.ToString("C"); } }

        public int Id { get; set; }

        private int stock;
        public int Stock
        {
            get { return stock; }
            set { stock = value >= 0 ? value : 0; }
        }
        public bool IsBogo { get; set; }

        public bool IsMarkedDown { get; set; }

        public decimal MarkedDownPrice { get; set; }

        public decimal MarkDown { get; set; }

        public ItemDTO(Item i)
        {
            Name = i.Name;
            Description = i.Description;
            Price = i.Price;
            Stock = i.Stock;
            Id = i.Id;
            IsBogo = i.IsBogo;
            IsMarkedDown = i.IsMarkedDown;
            MarkedDownPrice = i.MarkedDownPrice;
            MarkDown = i.MarkDown;
        }

        public ItemDTO(ItemDTO i)
        {
            Name = i.Name;
            Description = i.Description;
            Price = i.Price;
            Stock = i.Stock;
            Id = i.Id;
            IsBogo = i.IsBogo;
            IsMarkedDown = i.IsMarkedDown;
            MarkedDownPrice = i.MarkedDownPrice;
            MarkDown = i.MarkDown;
        }
        public ItemDTO()
        { }

    }
}
