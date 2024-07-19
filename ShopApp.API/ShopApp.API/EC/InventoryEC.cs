using ShopApp.API.Database;
using ShopApp.Maui.DTO;
using ShopAppLib.Models;
using System.Collections.ObjectModel;

namespace ShopApp.API.EC
{
    public class InventoryEC
    {

        private List<ItemDTO>? items = new List<ItemDTO>();
        public ReadOnlyCollection<ItemDTO>? Items
        {
            get
            {
                return items?.AsReadOnly();
            }
        }


        public InventoryEC()
        {
        }

        public async Task<IEnumerable<ItemDTO>> Get()
        {
            return FakeDatabase.Items.Take(100).Select(i => new ItemDTO(i));
        }

        public async Task<ItemDTO> AddOrUpdate(ItemDTO i)
        {
            if (items == null)
            {
                return null;
            }
            var isAdd = false;

            if (i.Id == 0)
            {
                i.Id = FakeDatabase.LastId + 1;
                isAdd = true;
            }

            if (isAdd)
            {
                FakeDatabase.Items.Add(new Item(i));
            }
            else
            {
                var existingItem = FakeDatabase.Items.FirstOrDefault(item => item.Id == i.Id);
                if (existingItem != null)
                {
                    var index = FakeDatabase.Items.IndexOf(existingItem);
                    FakeDatabase.Items.RemoveAt(index);
                    existingItem = new Item(i);
                    FakeDatabase.Items.Insert(index, existingItem);

                }
            }
            return i;
        }

        public async Task<ItemDTO> Delete(int id)
        {
            if (items == null)
            {
                return null;
            }
            var itemToDelete = FakeDatabase.Items.FirstOrDefault(c => c.Id == id);

            if (itemToDelete != null)
            {
                FakeDatabase.Items.Remove(itemToDelete);
            }

            return new ItemDTO(itemToDelete ?? new Item());
        }

        public async Task<IEnumerable<ItemDTO>> Search(string? query)
        {
            return FakeDatabase.Items.Where(i =>
            i.Name.ToUpper().Contains(query?.ToUpper() ?? string.Empty)
            || i.Description.ToUpper().Contains(query?.ToUpper() ?? string.Empty)).Take(100).Select(i => new ItemDTO(i));

            ;
        }


    }
}

