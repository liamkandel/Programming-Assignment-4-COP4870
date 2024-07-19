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

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }


        [HttpGet()]
        public async Task<IEnumerable<ItemDTO>> Get()
        {
            return await new InventoryEC().Get();
        }

        [HttpPost("/Search")]

        public async Task<IEnumerable<ItemDTO>> Get(Query query)
        {
            return await new InventoryEC().Search(query.QueryString);
        }

        [HttpDelete("/{id}")]
        public async Task<ItemDTO> Delete(int id)
        {
            return await new InventoryEC().Delete(id);
        }

        [HttpPost()]

        public async Task<ItemDTO> AddOrUpdate([FromBody] ItemDTO i)
        {

            return await new InventoryEC().AddOrUpdate(i);
        }
    }
}
