using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeon_server.Models;
using MongoDB.Driver;

namespace Pizzeon_server {
	public class MongoDBRepository : IRepository {

		private MongoClient client;
		private IMongoDatabase database;
		private IMongoCollection<Player> PlayerCollection;
		private IMongoCollection<Inventory> InventoryCollection;
		private IMongoCollection<Hat> HatCollection;
		private IMongoCollection<Color> ColorCollection;
		private IMongoCollection<Avatar> AvatarCollection;
		public MongoDBRepository () {
			client = new MongoClient("mongodb://localhost:27017");
			database = client.GetDatabase("pizzeon");
			PlayerCollection = database.GetCollection<Player>("players");
			InventoryCollection = database.GetCollection<Inventory>("inventories");
			HatCollection = database.GetCollection<Hat>("hats");
			ColorCollection = database.GetCollection<Color>("colors");
			AvatarCollection = database.GetCollection<Avatar>("avatars");

		}

		public async Task AddAvatarToInventory (Guid playerid, Guid avatarid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedAvatars", avatarid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddColorToInventory (Guid playerid, Guid colorid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedColors", colorid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddHatToInventory (Guid playerid, Guid hatid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedHats", hatid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddStatsMulti (Guid playerid, SessionStatsMulti stats) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			PlayerStatsMulti multiStats = player.MultiStats;
			multiStats.PlayedGames++;
			multiStats.AllPoints += stats.Points;
			multiStats.Distance += stats.Distance;
			multiStats.Crashes += stats.Crashes;
			multiStats.Dropped += stats.Dropped;
			multiStats.Hits += stats.Hits;
			if (stats.Win) {
				multiStats.Win++;
			} else {
				multiStats.Loss++;
			}
			multiStats.PinpointAccuracy += stats.PinpointAccuracy;
			multiStats.OverallGameTime += stats.GameTime;
			if (stats.Points > multiStats.BestPoints) {
				multiStats.BestPoints = stats.Points;
			}
			var update = Builders<Player>.Update.Set("MultiStats", multiStats);
			await PlayerCollection.UpdateOneAsync(filter, update);

		}

		public async Task AddStatsSingle (Guid playerid, SessionStatsSingle stats) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			PlayerStatsSingle singleStats = player.SingleStats;
			singleStats.PlayedGames++;
			singleStats.AllPoints += stats.Points;
			singleStats.Distance += stats.Distance;
			singleStats.Crashes += stats.Crashes;
			singleStats.Dropped += stats.Dropped;
			singleStats.Hits += stats.Hits;
			singleStats.PinpointAccuracy += stats.PinpointAccuracy;
			singleStats.OverallGameTime += stats.GameTime;
			if (stats.GameTime > singleStats.BestGameTime) {
				singleStats.BestGameTime = stats.GameTime;
			}
			if (stats.Points > singleStats.BestPoints) {
				singleStats.BestPoints = stats.Points;
			}
			var update = Builders<Player>.Update.Set("SingleStats", singleStats);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public async Task<bool> CheckUsernameAvailable (string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await PlayerCollection.FindAsync(filter);
			if (cursor.Any()) {
				return false;
			} else {
				return true;
			}
		}

		public Task CreateAvatar (Avatar avatar) {
			return AvatarCollection.InsertOneAsync(avatar);
		}

		public Task CreateColor (Color color) {
			return ColorCollection.InsertOneAsync(color);
		}

		public Task CreateHat (Hat hat) {
			return HatCollection.InsertOneAsync(hat);
		}

		public Task CreateInventory (Inventory inventory) {
			return InventoryCollection.InsertOneAsync(inventory);
		}

		public Task CreatePlayer (Player player) {
			return PlayerCollection.InsertOneAsync(player);
		}

		public async Task DeductCoinFromPlayer (Guid playerId, int price) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", -price);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public async Task<Avatar[]> GetAllAvatars() {
			var filter = Builders<Avatar>.Filter.Empty;
			var cursor = await AvatarCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public async Task<Color[]> GetAllColors() {
			var filter = Builders<Color>.Filter.Empty;
			var cursor = await ColorCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public async Task<Hat[]> GetAllHats() {
			var filter = Builders<Hat>.Filter.Empty;
			var cursor = await HatCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public async Task<Avatar> GetAvatar (Guid Id) {
			var filter = Builders<Avatar>.Filter.Eq("Id", Id);
			var cursor = await AvatarCollection.FindAsync(filter);
			Avatar avatar = cursor.Single();
			return avatar;
		}

		public async Task<Color> GetColor (Guid Id) {
			var filter = Builders<Color>.Filter.Eq("Id", Id);
			var cursor = await ColorCollection.FindAsync(filter);
			Color color = cursor.Single();
			return color;
		}

		public async Task<Hat> GetHat (Guid Id) {
			var filter = Builders<Hat>.Filter.Eq("Id", Id);
			var cursor = await HatCollection.FindAsync(filter);
			Hat hat = cursor.Single();
			return hat;
		}

		public async Task<Inventory> GetInventory (Guid playerId) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerId);
			var cursor = await InventoryCollection.FindAsync(filter);
			Inventory inventory = cursor.Single();
			return inventory;
		}

		public async Task<PlayerStatsMulti> GetMultiStats (Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player.MultiStats;
		}

		public async Task<Player> GetPlayer (Guid Id) {
			var filter = Builders<Player>.Filter.Eq("Id", Id);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player;
		}

		public async Task<Player> GetPlayerByName (string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.SingleOrDefault();
			return player;
		}

		public async Task<PlayerStatsSingle> GetSingleStats (Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player.SingleStats;
		}

		public async Task RemoveAvatar (Guid Id) {
			var filter = Builders<Avatar>.Filter.Eq("Id", Id);
			await AvatarCollection.DeleteOneAsync(filter);
		}

		public async Task RemoveColor (Guid Id) {
			var filter = Builders<Color>.Filter.Eq("Id", Id);
			await ColorCollection.DeleteOneAsync(filter);
		}

		public async Task RemoveHat (Guid Id) {
			var filter = Builders<Hat>.Filter.Eq("Id", Id);
			await HatCollection.DeleteOneAsync(filter);
		}

		public async Task RemoveInventory (Guid Id) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", Id);
			await InventoryCollection.DeleteOneAsync(filter);
		}

		public async Task RemovePlayer (Guid Id) {
			var filter = Builders<Player>.Filter.Eq("Id", Id);
			await PlayerCollection.DeleteOneAsync(filter);
		}
	}
}
