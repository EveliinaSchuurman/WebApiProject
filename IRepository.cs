using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace WebApiProject{
    public interface IRepository
    {
        Task<Player> CreatePlayer(Player player);
        Task<Player> GetPlayer(Guid playerId);
        Task<Player> GetPlayer(string Name);
        Task<Player[]> GetAllPlayers();
        Task<Player> UpdatePlayer(Guid id, ModifiedPlayer player);
        Task<Player> DeletePlayer(Guid playerId);
        Task<Player[]> GetHigherLevelPlayers(int Score);
        Task<Player[]> GetPlayersWithTag(Tag tag);
        Task<Player[]> GetPlayersWithItem(ItemType type);
        Task<Player[]> GetPlayersWithXAmountOfItems(int size);
        Task<UpdateResult> UpdatePlayerName(Guid id, string player);
        Task<UpdateResult> UpdatePlayerScore(Guid id, int AddToScore);
        Task<UpdateResult> UpdatePlayerItemList(Guid id, Item item);
        Task<Player> SellForScore(Guid id, Guid itemId);
        Task<Player[]> GetTop10Players();


        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Item item);
        Task<Item> DeleteItem(Guid playerId, Item item);
    }
    
}