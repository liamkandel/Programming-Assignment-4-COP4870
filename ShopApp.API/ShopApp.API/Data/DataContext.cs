using Microsoft.EntityFrameworkCore;
using ShopAppLib.Models;

namespace ShopApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
