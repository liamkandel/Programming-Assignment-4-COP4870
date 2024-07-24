// Liam Kandel
using Newtonsoft.Json;
using ShopApp.Maui.DTO;
using ShopAppLib.Utilities;
using System.Collections.ObjectModel;

namespace ShopAppLib
{
    public class InventoryServiceProxy
    {

        private InventoryServiceProxy()
        {

            Get();
        }

        private static InventoryServiceProxy? instance;
        private static object instanceLock = new object();
        public static InventoryServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryServiceProxy();
                    }
                }

                return instance;
            }
        }

        private List<ItemDTO>? items;
        public ReadOnlyCollection<ItemDTO>? Items
        {
            get
            {
                return items?.AsReadOnly();
            }
        }

        //======== functionality
        public int LastId
        {
            get
            {
                if (items?.Any() ?? false)
                {
                    return items?.Select(c => c.Id)?.Max() ?? 0;
                }
                return 0;
            }
        }

        public decimal TaxRate { get; set; }

        public async Task<ItemDTO> AddOrUpdate(ItemDTO? item)
        {
            var response = await new WebRequestHandler().Post("/Inventory", item);
            return JsonConvert.DeserializeObject<ItemDTO>(response);
        }

        public async Task<IEnumerable<ItemDTO>> Get()
        {
            var response = await new WebRequestHandler().Get("/Inventory");
            if (response == null) { items = null; }
            items = JsonConvert.DeserializeObject<List<ItemDTO>>(response) ?? new List<ItemDTO>();
            return items;
        }

        public async Task<ItemDTO?> Delete(int id)
        {
            var response = await new WebRequestHandler().Delete($"/{id}");
            var itemToDelete = JsonConvert.DeserializeObject<ItemDTO>(response);
            return itemToDelete;
        }
        public async Task<IEnumerable<ItemDTO>> Search(Query query)
        {
            if (query == null)
            {
                return await Get();
            }
            var response = await new WebRequestHandler().Post("/Search", query);
            items = JsonConvert.DeserializeObject<List<ItemDTO>>(response);
            return items;

        }
    }
}
