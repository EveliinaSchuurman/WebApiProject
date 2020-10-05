using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiProject.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IRepository _irepository;

        public PlayerController(ILogger<PlayerController> logger, IRepository irepository)
        {
            _logger = logger;
            _irepository = irepository;
        }

        [HttpPost] //{"Name":"yeet"}
        [Route("create")]
        public async Task<Player> Create([FromBody] NewPlayer player)
        {
            DateTime localDate = DateTime.Now;

            Player new_player = new Player();
            new_player.Name = player.Name;
            new_player.Id = Guid.NewGuid();
            new_player.Score = 0;
            new_player.Level = 0;
            new_player.Active = player.Tag;
            new_player.CreationTime = localDate;
            new_player.ItemScore = 0;

            await _irepository.CreatePlayer(new_player);
            return new_player;
        }
        
        [HttpGet]
        [Route("ListPlayers")]
        public Task<Player[]> GetAll()
        {
            Task<Player[]> list_players = _irepository.GetAllPlayers();
            return list_players;
        }

        [HttpGet]
        [Route("Delete/{id:Guid}")]
        public async Task<Player> Delete(Guid id)
        {
            await _irepository.DeletePlayer(id);
            return null;
        }


        [HttpGet]
        [Route("Get/{id:Guid}")]
        public async Task<Player> GetPlayer(Guid id)
        {
            return await _irepository.GetPlayer(id);
        }

        [HttpPost] //{"Score":5}
        [Route("Modify/{id:Guid}")]
        public async Task<Player> UpdatePlayer(Guid id, [FromBody] ModifiedPlayer player)
        {
            await _irepository.UpdatePlayer(id, player);
            return null;
        }

        //5.1
        [HttpGet]
        [Route("GetHigherLeverPlayers")]
        public async Task<Player[]>GetHigherLevelPlayers(int minScore){
            return await _irepository.GetHigherLevelPlayers(minScore);
        }
        //5.2
        [HttpGet]
        [Route("Get/{Name:string}")]
        public async Task<Player> GetPlayer(string Name)
        {
            return await _irepository.GetPlayer(Name);
        }
        //5.3
        [HttpGet]
        [Route("Get/{tag:Tag}")]
        public async Task<Player[]> GetPlayersWithTag(Tag tag)
        {
            return await _irepository.GetPlayersWithTag(tag);
        }
        //5.4
        [HttpGet]
        [Route("Get/{type:Itemtype}")]
        public async Task<Player[]> GetPlayersWithItem(ItemType type)
        {
            return await _irepository.GetPlayersWithItem(type);
        }
        //5.5
        [HttpGet]
        [Route("Get/{size:int}")]
        public async Task<Player[]>GetPlayersWithXAmountOfItems(int size){
            return await _irepository.GetPlayersWithXAmountOfItems(size);
        }
        //5.6
        [HttpPost] //{"Name":Gwent}
        [Route("Namechange/{id:Guid}")]
        public async Task<Player> UpdatePlayerName(Guid id, [FromBody] string player)
        {
            await _irepository.UpdatePlayerName(id, player);
            return null;
        }
        //5.7
        [HttpPost] //{"AddToScore: 4}
        [Route("Scorechange/{id:Guid}")]
        public async Task<Player> UpdatePlayerScore(Guid id, [FromBody] int AddToScore)
        {
            await _irepository.UpdatePlayerScore(id, AddToScore);
            return null;
        }
        //5.8
        [HttpPost] //{"Item: item}
        [Route("Scorechange/{id:Guid}")]
        public async Task<Player> UpdatePlayerItemList(Guid id, [FromBody] Item item)
        {
            await _irepository.UpdatePlayerItemList(id, item);
            return null;
        }
        //5.9
        [HttpDelete]
        [Route("{player_id:Guid}/items/{itemid:Guid}")]
        public async Task<Player> SellForScore(Guid player_id, Guid itemid)
        {
            await _irepository.SellForScore(player_id, itemid);
            return null;
        }
        //5.10
        [HttpGet]
        [Route("GetTop10Players")]
        public async Task<Player[]> GetTop10Players()
        {
            return await _irepository.GetTop10Players();
        }

        [HttpGet]
        [Route("GetTopxPlayers/{Amount:int}")]
        public async Task<Player[]> GetTopXPlayers(int Amount)
        {
            return await _irepository.GetTopXPlayers(Amount);
        }
        [HttpGet]
        [Route("GetTopPlayer/{Xth:int}")]
        public async Task<Player> GetTopPlayer(int Xth){
            return await _irepository.GetTopPlayer(Xth);
        }
    }
}
