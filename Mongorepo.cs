using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApiProject
{
    public class MongoDBrepo : IRepository
    {
        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDBrepo()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("game"); //muuta
            _playerCollection = database.GetCollection<Player>("players");

            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            await _playerCollection.InsertOneAsync(player);
            return player;
        }

        public async Task<Player> DeletePlayer(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            return await _playerCollection.FindOneAndDeleteAsync(filter);
        }

        public async Task<Player> GetPlayer(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
            return await _playerCollection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetAllPlayers()
        {
            var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> UpdatePlayer(Guid id, ModifiedPlayer player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            Player returnPlayer = await _playerCollection.Find(filter).FirstAsync();
            returnPlayer.Score = player.Score;
            await _playerCollection.ReplaceOneAsync(filter, returnPlayer);
            return returnPlayer;
        }
        //Get Players who have more than x score yritys
        public async Task<Player[]> GetHigherLevelPlayers(int Score){
            FilterDefinition<Player> filter = Builders<Player>.Filter.Gt("Score", Score);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> GetPlayer(string Name)
        {
            var filter = Builders<Player>.Filter.Eq("Name", Name);
            return await _playerCollection.Find(filter).FirstAsync();
        }
        public async Task<Player[]> GetPlayersWithTag(Tag tag){
            var filter = Builders<Player>.Filter.Eq(player => player.Active, tag);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();
            return players.ToArray();
        }
        public async Task<Player[]> GetPlayersWithItem(ItemType type){
            var playersWithWeapons = Builders<Player>.Filter.ElemMatch<Item>(p => p.itemList, 
            Builders<Item>.Filter.Eq(i => i.Type, ItemType.POTION));
            List<Player> players =  await _playerCollection.Find(playersWithWeapons).ToListAsync();
            return players.ToArray();
        }
        public async Task<Player[]> GetPlayersWithXAmountOfItems(int size){
            var filter = Builders<Player>.Filter.Size(p => p.itemList, size);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();
            return players.ToArray();
        }
        public async Task<UpdateResult> UpdatePlayerName(Guid id, string player)
        {
            /*FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            Player returnPlayer = await _playerCollection.Find(filter).FirstAsync();
            returnPlayer.Name = player.Name;
            await _playerCollection.ReplaceOneAsync(filter, returnPlayer);
            return returnPlayer;*/
             var filter = Builders<Player>.Filter.Eq("Id", id);
            var update = Builders<Player>.Update.Set("Name", player);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }
        public async Task<UpdateResult> UpdatePlayerScore(Guid id, int AddToScore){
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("Id", id);
            var update = Builders<Player>.Update.Inc("Score", AddToScore);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }
        public async Task<UpdateResult> UpdatePlayerItemList(Guid id, Item item){
            var filter = Builders<Player>.Filter.Eq("Id", id);
            var update = Builders<Player>.Update.Push("itemList", item);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }
        public async Task<Player> SellForScore(Guid id, Guid itemId){
            /*FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            Player returnPlayer = await _playerCollection.Find(filter).FirstAsync();
            var itemFilter = Builders<Item>.Filter.Eq(item => item.Id, itemId);
            Player player = await _playerCollection.Find(itemFilter).FirstAsync();

            for (int i = 0; i < player.itemList.Count; i++)
            {
                if (player.itemList[i].Id == itemId)
                    return player.itemList[i];
            }

            return null;*/
             var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var updateScore = Builders<Player>.Update.Inc("Score", 4);
            await _playerCollection.FindOneAndUpdateAsync(filter, updateScore);

            var filterItem = Builders<Player>.Filter.ElemMatch<Item>(p => p.itemList, Builders<Item>.Filter.Eq(i => i.Id, itemId));
            return await _playerCollection.FindOneAndDeleteAsync(filterItem);
        }
         public async Task<Player[]> GetTop10Players()
        {
            var filter = Builders<Player>.Filter.Empty;
            SortDefinition<Player> sortDef = Builders<Player>.Sort.Descending("Score");
            List<Player> players = await _playerCollection.Find(filter).Sort(sortDef).Limit(10).ToListAsync();
            return players.ToArray();
        }



        public async Task<Player[]> GetTopXPlayers(int Amount)
        {
            var filter = Builders<Player>.Filter.Empty;
            SortDefinition<Player> sortDef = Builders<Player>.Sort.Descending("Score");
            List<Player> players = await _playerCollection.Find(filter).Sort(sortDef).Limit(Amount).ToListAsync();
            return players.ToArray();
        }
        public async Task<Player> GetXthPlayer(int Xth)
        {
            var filter = Builders<Player>.Filter.Empty;
            SortDefinition<Player> sortDef = Builders<Player>.Sort.Descending("Score");
            List<Player> players = await _playerCollection.Find(filter).Sort(sortDef).Limit(1).ToListAsync();
            players.ToArray();
            return players[Xth -1];
        }
        public async Task<Player> GetPlayerWithBestItem(){
            return null;
        }






        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            Player player = await GetPlayer(playerId);
            if(player == null){
                throw new NotFoundException();
            }else{
                player.itemList.Add(item);
                var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
                await _playerCollection.ReplaceOneAsync(filter, player);
                return item;
            }
        }
        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player player = await GetPlayer(playerId);
            //var filter = Builders<Item>.Filter.Eq(item => item.Id, itemId);

            for (int i = 0; i < player.itemList.Count; i++)
            {
                if (player.itemList[i].Id == itemId)
                    return player.itemList[i];
            }

            return null;
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player player = await GetPlayer(playerId);
            return player.itemList.ToArray();
        }

        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            Player player = await GetPlayer(playerId);

            foreach (var it in player.itemList)
            {
                if (it.Id == item.Id)
                {
                    it.Level = item.Level;
                    var filter_player = Builders<Player>.Filter.Eq(player => player.Id, playerId);
                    await _playerCollection.ReplaceOneAsync(filter_player, player);
                    return it;
                }
            }

            return null;
        }
        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            Player player = await GetPlayer(playerId);

            for (int i = 0; i < player.itemList.Count; i++)
            {
                if (player.itemList[i].Id == item.Id)
                {
                    player.itemList.RemoveAt(i);
                    var filter_player = Builders<Player>.Filter.Eq(player => player.Id, playerId);
                    await _playerCollection.ReplaceOneAsync(filter_player, player);
                    return item;
                }
            }

            return null;
        }
        
    }
}