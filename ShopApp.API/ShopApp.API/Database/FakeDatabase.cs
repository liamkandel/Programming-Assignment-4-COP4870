using ShopAppLib.Models;

namespace ShopApp.API.Database
{
    public static class FakeDatabase
    {
        public static int LastId
        {
            get
            {
                if (Items?.Any() ?? false)
                {
                    return Items?.Select(c => c.Id)?.Max() ?? 0;
                }
                return 0;
            }
        }
        public static List<Item> Items { get; } = new List<Item>()
            {
                new Item{Name="Speaker", Description="on proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", Price=300.5M, Stock=6, Id=5, IsBogo=false, IsMarkedDown = false },
                new Item{Name="Notebook", Description="t in culpa qui officia deserunt mollit anim id est laborum.", Price=300.5M, Stock=-4, Id=6, IsBogo=false, IsMarkedDown=false},
            };

    }
}
