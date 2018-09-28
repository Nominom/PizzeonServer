using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeon_server.Models;
using MongoDB.Driver;

namespace Pizzeon_server
{
    public class MongoDBRepository : IRepository
    {

        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Player> PlayerCollection;
        private IMongoCollection<Inventory> InventoryCollection;
        private IMongoCollection<Hat> HatCollection;
        private IMongoCollection<Color> ColorCollection;
        private IMongoCollection<Avatar> AvatarCollection;
        public MongoDBRepository() {
            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("pizzeon");
            PlayerCollection = database.GetCollection<Player>("players");
            InventoryCollection = database.GetCollection<Inventory>("inventories");
            HatCollection = database.GetCollection<Hat>("hats");
            ColorCollection = database.GetCollection<Color>("colors");
            AvatarCollection = database.GetCollection<Avatar>("avatars");
            
        }

        public Task AddAvatarToInventory(Guid playerid, Guid avatarid)
        {
            throw new NotImplementedException();
        }

        public Task AddColorToInventory(Guid playerid, Guid colorid)
        {
            throw new NotImplementedException();
        }

        public Task AddHatToInventory(Guid playerid, Guid hatid)
        {
            throw new NotImplementedException();
        }

        public Task AddStatsMulti(Guid playerid, SessionStatsMulti stats)
        {
            throw new NotImplementedException();
        }

        public Task AddStatsSingle(Guid playerid, SessionStatsSingle stats)
        {
            throw new NotImplementedException();
        }

        public Task CreateAvatar(Avatar avatar)
        {
            return AvatarCollection.InsertOneAsync(avatar);
        }

        public Task CreateColor(Color color)
        {
            return ColorCollection.InsertOneAsync(color);
        }

        public Task CreateHat(Hat hat)
        {
            return HatCollection.InsertOneAsync(hat);
        }

        public Task CreateInventory(Inventory inventory)
        {
            return InventoryCollection.InsertOneAsync(inventory);
        }

        public Task CreatePlayer(Player player)
        {
            return PlayerCollection.InsertOneAsync(player);
        }

        public Task DeductCoinFromPlayer(Guid playerId, int price)
        {
            throw new NotImplementedException();
        }

        public async Task<Avatar> GetAvatar(Guid Id)
        {
            var filter = Builders<Avatar>.Filter.Eq("Id", Id);
            var cursor = await AvatarCollection.FindAsync(filter);
            Avatar avatar = cursor.Single();
            return avatar;
        }

        public async Task<Color> GetColor(Guid Id)
        {
            var filter = Builders<Color>.Filter.Eq("Id", Id);
            var cursor = await ColorCollection.FindAsync(filter);
            Color color = cursor.Single();
            return color;        
        }

        public async Task<Hat> GetHat(Guid Id)
        {
            var filter = Builders<Hat>.Filter.Eq("Id", Id);
            var cursor = await HatCollection.FindAsync(filter);
            Hat hat = cursor.Single();
            return hat;
        }

        public async Task<Inventory> GetInventory(Guid playerId)
        {
            var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerId);
            var cursor = await InventoryCollection.FindAsync(filter);
            Inventory inventory = cursor.Single();
            return inventory;
        }

        public Task<PlayerStatsMulti> GetMultiStats(Guid playerid)
        {
            throw new NotImplementedException();
        }

        public Task<Player> GetPlayer(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<PlayerStatsSingle> GetSingleStats(Guid playerid)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAvatar(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveColor(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveHat(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveInventory(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task RemovePlayer(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
