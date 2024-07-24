using Microsoft.AspNetCore.Mvc;
using ShopApp.API.EC;
using ShopApp.Maui.DTO;
using ShopAppLib.Utilities;

namespace ShopApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {


        private readonly ILogger<InventoryController> _logger;
        private readonly InventoryEC _inventoryEC;

        public InventoryController(ILogger<InventoryController> logger, InventoryEC InventoryEC)
        {
            _logger = logger;
            _inventoryEC = InventoryEC;
        }


        [HttpGet()]
        public async Task<IEnumerable<ItemDTO>> Get()
        {
            return await _inventoryEC.Get();
        }

        [HttpPost("/Search")]

        public async Task<IEnumerable<ItemDTO>> Get(Query query)
        {
            return await _inventoryEC.Search(query.QueryString);
        }

        [HttpPost("/Inventory")]

        public async Task<ItemDTO> AddOrUpdate([FromBody] ItemDTO i)
        {
            return await _inventoryEC.AddOrUpdate(i);
        }

        [HttpDelete("/{id}")]
        public async Task<ItemDTO> Delete(int id)
        {
            return await _inventoryEC.Delete(id);
        }


    }
}
