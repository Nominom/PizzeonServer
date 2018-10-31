using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Pizzeon_server.Models;

namespace PizzeonAzureFunctions {
	public static class MongoDbRepository {

		public const string DbName = "pizzeon";

		private static IMongoCollection<Player> PlayerCollection => GetCollection<Player>("players");
		private static IMongoCollection<Inventory> InventoryCollection => GetCollection<Inventory>("inventories");
		private static IMongoCollection<Hat> HatCollection => GetCollection<Hat>("hats");
		private static IMongoCollection<Color> ColorCollection => GetCollection<Color>("colors");
		private static IMongoCollection<Avatar> AvatarCollection => GetCollection<Avatar>("avatars");

		private static IMongoCollection<T> GetCollection<T> (string collectionName) {
			MongoClient client = new MongoClient(System.Environment.GetEnvironmentVariable("MongoDBConnectionString"));
			IMongoDatabase db = client.GetDatabase(DbName);

			var collection = db.GetCollection<T>(collectionName);
			return collection;
		}

		public static Task CreatePlayer (Player player) {
			return PlayerCollection.InsertOneAsync(player);
		}

		public static async Task<Player> GetPlayer (Guid id) {
			var filter = Builders<Player>.Filter.Eq("Id", id);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player;
		}

		public static async Task<Player> GetPlayerByName (string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.SingleOrDefault();
			return player;
		}

		public static async Task<bool> CheckUsernameAvailable (string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await PlayerCollection.FindAsync(filter);
			if (cursor.Any()) {
				return false;
			}

			return true;
		}

		public static async Task DeductCoinFromPlayer (Guid playerId, int price) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", -price);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task AddCoinToPlayer (Guid playerId, int coin) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", +coin);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}



		public static async Task CreateAvatar (Avatar avatar) {
			var filter = Builders<Avatar>.Filter.Eq("Id", avatar.Id);
			var existingAvatar = await AvatarCollection.FindAsync(filter);

			if (!existingAvatar.Any()) {
				await AvatarCollection.InsertOneAsync(avatar);
			} else {
				await AvatarCollection.ReplaceOneAsync(filter, avatar);
			}
		}

		public static async Task CreateColor (Color color) {
			var filter = Builders<Color>.Filter.Eq("Id", color.Id);
			var existingColor = await ColorCollection.FindAsync(filter);

			if (!existingColor.Any()) {
				await ColorCollection.InsertOneAsync(color);
			} else {
				await ColorCollection.ReplaceOneAsync(filter, color);
			}
		}

		public static async Task CreateHat (Hat hat) {
			var filter = Builders<Hat>.Filter.Eq("Id", hat.Id);
			var existingHat = await HatCollection.FindAsync(filter);

			if (!existingHat.Any()) {
				await HatCollection.InsertOneAsync(hat);
			} else {
				await HatCollection.ReplaceOneAsync(filter, hat);
			}
		}

		public static async Task<Avatar[]> GetAllAvatars () {
			var filter = Builders<Avatar>.Filter.Empty;
			var cursor = await AvatarCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public static async Task<Color[]> GetAllColors () {
			var filter = Builders<Color>.Filter.Empty;
			var cursor = await ColorCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public static async Task<Hat[]> GetAllHats () {
			var filter = Builders<Hat>.Filter.Empty;
			var cursor = await HatCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public static Task CreateInventory (Inventory inventory) {
			return InventoryCollection.InsertOneAsync(inventory);
		}

		public static async Task<Inventory> GetInventory (Guid playerId) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerId);
			var cursor = await InventoryCollection.FindAsync(filter);
			Inventory inventory = cursor.Single();
			return inventory;
		}

		public static async Task AddAvatarToInventory (Guid playerid, string avatarid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedAvatars", avatarid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public static async Task AddColorToInventory (Guid playerid, string colorid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedColors", colorid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public static async Task AddHatToInventory (Guid playerid, string hatid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedHats", hatid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public static async Task<bool> InventoryHasHat (Guid playerId, string hatId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId),
				Builders<Inventory>.Filter.Where(x => x.OwnedHats.Contains(hatId)));

			return (await InventoryCollection.FindAsync(filter)).Any();
		}

		public static async Task<bool> InventoryHasAvatar (Guid playerId, string avatarId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId),
				Builders<Inventory>.Filter.Where(x => x.OwnedAvatars.Contains(avatarId)));

			return (await InventoryCollection.FindAsync(filter)).Any();
		}

		public static async Task<bool> InventoryHasColor (Guid playerId, string colorId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId),
				Builders<Inventory>.Filter.Where(x => x.OwnedColors.Contains(colorId)));

			return (await InventoryCollection.FindAsync(filter)).Any();
		}

		public static async Task EquipHat (Guid playerId, string hatId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Hat", hatId);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task EquipAvatar (Guid playerId, string avatarId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Avatar", avatarId);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task EquipColor (Guid playerId, string colorId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Color", colorId);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task<Avatar> GetAvatar (string id) {
			var filter = Builders<Avatar>.Filter.Eq("Id", id);
			var cursor = await AvatarCollection.FindAsync(filter);
			Avatar avatar = cursor.Single();
			return avatar;
		}

		public static async Task<Color> GetColor (string id) {
			var filter = Builders<Color>.Filter.Eq("Id", id);
			var cursor = await ColorCollection.FindAsync(filter);
			Color color = cursor.Single();
			return color;
		}

		public static async Task<Hat> GetHat (string id) {
			var filter = Builders<Hat>.Filter.Eq("Id", id);
			var cursor = await HatCollection.FindAsync(filter);
			Hat hat = cursor.Single();
			return hat;
		}


		public static async Task<PlayerStatsSingle> GetSingleStats (Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player.SingleStats;
		}


		public static async Task<PlayerStatsMulti> GetMultiStats (Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.SingleOrDefault();
			return player.MultiStats;
		}

		public static async Task AddStatsSingle (Guid playerid, SessionStatsSingle stats) {
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

		public static async Task AddStatsMulti (Guid playerid, SessionStatsMulti stats) {
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


		public static async Task<IEnumerable<PlayerStatsView>> GetTopPlayerStatsSingle (int number, int page) {
			var sort = Builders<Player>.Sort.Descending(x => x.SingleStats.BestPoints);
			var aggregate = PlayerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.SingleStats.PlayedGames > 0)
				.Skip(page * number)
				.Limit(number)
				.Project(x => new PlayerStatsView() {
					Username = x.Username,
					Pizzeria = x.Pizzeria,
					Accuracy = x.SingleStats.Dropped == 0 ? 0 : (x.SingleStats.PinpointAccuracy / (float)x.SingleStats.Dropped),
					AllPoints = x.SingleStats.AllPoints,
					BestPoints = x.SingleStats.BestPoints,
					Distance = x.SingleStats.Distance,
					PizzasDelivered = x.SingleStats.Hits,
					PlayedGames = x.SingleStats.PlayedGames,
					PlayTime = x.SingleStats.OverallGameTime
				});
			return await aggregate.ToListAsync();
		}

		public static async Task<IEnumerable<PlayerStatsView>> GetTopPlayerStatsMulti (int number, int page) {
			var sort = Builders<Player>.Sort.Descending(x => x.MultiStats.BestPoints);
			var aggregate = PlayerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.MultiStats.PlayedGames > 0)
				.Skip(page * number)
				.Limit(number)
				.Project(x => new PlayerStatsView() {
					Username = x.Username,
					Pizzeria = x.Pizzeria,
					Accuracy = x.MultiStats.PinpointAccuracy / x.MultiStats.Dropped,
					AllPoints = x.MultiStats.AllPoints,
					BestPoints = x.MultiStats.BestPoints,
					Distance = x.MultiStats.Distance,
					PizzasDelivered = x.MultiStats.Hits,
					PlayedGames = x.MultiStats.PlayedGames,
					PlayTime = x.MultiStats.OverallGameTime
				});
			return await aggregate.ToListAsync();
		}
	}
}
