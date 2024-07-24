using Microsoft.EntityFrameworkCore;
using ShopApp.API.Data;
using ShopApp.Maui.DTO;
using ShopAppLib.Models;
using System.Collections.ObjectModel;

namespace ShopApp.API.EC
{
    public class InventoryEC
    {
        private readonly DataContext _context;
        public InventoryEC(DataContext context)
        {
            _context = context;
        }

        private List<ItemDTO>? items = new List<ItemDTO>();
        public ReadOnlyCollection<ItemDTO>? Items
        {
            get
            {
                return items?.AsReadOnly();
            }
        }


        public async Task<IEnumerable<ItemDTO>> Get()
        {
            var items = await _context.Items.ToListAsync();
            return items.Take(100).Select(i => new ItemDTO(i));
        }

        public async Task<ItemDTO> AddOrUpdate(ItemDTO i)
        {
            var item = await _context.Items.FindAsync(i.Id);
            if (item == null)
            {
                _context.Items.Add(new Item(i));
            }
            else
            {
                item.Name = i.Name;
                item.Description = i.Description;
                item.Price = i.Price;
                item.Stock = i.Stock;
                item.IsMarkedDown = i.IsMarkedDown;
                item.MarkedDownPrice = i.MarkedDownPrice;
                item.MarkDown = i.MarkDown;
                item.IsBogo = i.IsBogo;
            }
            await _context.SaveChangesAsync();
            return i;

        }

        public async Task<ItemDTO> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return new ItemDTO();
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return new ItemDTO(item);
        }

        public async Task<IEnumerable<ItemDTO>> Search(string? query)
        {
            var items = await _context.Items.ToListAsync();

            var filteredItems = items.Where(i =>
                                (i.Name ?? string.Empty).ToUpper().Contains(query?.ToUpper() ?? string.Empty)
                                || (i.Description ?? string.Empty).ToUpper().Contains(query?.ToUpper() ?? string.Empty))
                                .Take(100)
                                .Select(i => new ItemDTO(i));
            return filteredItems;


        }



    }
}

