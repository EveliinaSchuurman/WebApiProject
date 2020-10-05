using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameWebApi.Controllers
{
    [ApiController]
    [Route("players/{playerId}/items")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IRepository _irepository;

        public ItemController(ILogger<ItemController> logger, IRepository irepository)
        {
            _logger = logger;
            _irepository = irepository;
        }

        [HttpPost] //{"Name":"yeet"}
        
        [Route("create")]
        public async Task<Item> CreateItem([FromBody] NewItem item, Guid playerId)
        {
            DateTime localDate = DateTime.UtcNow;

            Item new_item = new Item();
            new_item.Name = item.Name;
            new_item.Id = Guid.NewGuid();
            
            new_item.Level = item.Level;
            new_item.Type = item.Type;
            new_item.CreationTime = localDate;

            await _irepository.CreateItem(playerId, new_item);
            return new_item;
        }



        [HttpGet]
        [Route("GetAllItems")]
        public Task<Item[]> GetAllItems(Guid playerId)
        {
            Task<Item[]> list_items = _irepository.GetAllItems(playerId);
            return list_items;
        }



        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            var delte_item = await _irepository.GetItem(playerId, item.Id);
            await _irepository.DeleteItem(playerId, delte_item);
            return null;
        }



        [HttpGet]
        [Route("GetItem/{id:Guid}")]
        public async Task<Item> GetItem(Guid id, Guid playerId)
        {
            return await _irepository.GetItem(playerId, id);
        }

        [HttpPost] //{"Score":5}
        [Route("UpdateItem")]
        public async Task<Item> UpdateItem([FromBody] Item item, Guid playerId)
        {
            await _irepository.UpdateItem(playerId, item);
            return null;
        }

    }
}