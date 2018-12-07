﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Bson;
using MongoDB.Driver;
using Pizzeon_server.Models;

namespace PizzeonAzureFunctions
{
	public static class MongoDbRepository {

		public const int leaderboardsCacheMinutes = 5;

		private static readonly MemoryCache Cache = MemoryCache.Default;

		public const string DbName = "pizzeon";

		private static IMongoCollection<Player> PlayerCollection => GetCollection<Player>("players");
		private static IMongoCollection<Inventory> InventoryCollection => GetCollection<Inventory>("inventories");
		private static IMongoCollection<Hat> HatCollection => GetCollection<Hat>("hats");
		private static IMongoCollection<Color> ColorCollection => GetCollection<Color>("colors");
		private static IMongoCollection<Avatar> AvatarCollection => GetCollection<Avatar>("avatars");
		private static IMongoCollection<PlayerAuthorizationToken> TokenCollection => GetCollection<PlayerAuthorizationToken>("tokens");

		private static IMongoCollection<T> GetCollection<T>(string collectionName) {
			MongoClient client = new MongoClient(System.Environment.GetEnvironmentVariable("MongoDBConnectionString"));
			IMongoDatabase db = client.GetDatabase(DbName);

			var collection = db.GetCollection<T>(collectionName);
			return collection;
		}

		public static Task CreatePlayer(Player player) {
			return PlayerCollection.InsertOneAsync(player);
		}

		public static async Task<Player> GetPlayer(Guid id) {
			var filter = Builders<Player>.Filter.Eq("Id", id);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player;
		}

		public static async Task<Player> GetPlayerByName(string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.SingleOrDefault();
			return player;
		}

		public static async Task<bool> CheckUsernameAvailable(string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await PlayerCollection.FindAsync(filter);
			if (cursor.Any()) {
				return false;
			}
			return true;
		}

		public static async Task DeductCoinFromPlayer(Guid playerId, int price) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", -price);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task AddCoinToPlayer(Guid playerId, int coin) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", +coin);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}



		public static async Task CreateAvatar(Avatar avatar) {
			var filter = Builders<Avatar>.Filter.Eq("Id", avatar.Id);
			var existingAvatar = await AvatarCollection.FindAsync(filter);

			if (!existingAvatar.Any()) {
				await AvatarCollection.InsertOneAsync(avatar);
			} else {
				await AvatarCollection.ReplaceOneAsync(filter, avatar);
			}
			ClearCache();
		}

		public static async Task CreateColor(Color color) {
			var filter = Builders<Color>.Filter.Eq("Id", color.Id);
			var existingColor = await ColorCollection.FindAsync(filter);

			if (!existingColor.Any()) {
				await ColorCollection.InsertOneAsync(color);
			} else {
				await ColorCollection.ReplaceOneAsync(filter, color);
			}
			ClearCache();
		}

		public static async Task CreateHat(Hat hat) {
			var filter = Builders<Hat>.Filter.Eq("Id", hat.Id);
			var existingHat = await HatCollection.FindAsync(filter);

			if (!existingHat.Any()) {
				await HatCollection.InsertOneAsync(hat);
			} else {
				await HatCollection.ReplaceOneAsync(filter, hat);
			}
			ClearCache();
		}

		public static async Task<Avatar[]> GetAllAvatars(TraceWriter log) {
			const string cacheKey = "allAvatars";
			Avatar[] avatars = Cache[cacheKey] as Avatar[];
			if (avatars == null) {
				var filter = Builders<Avatar>.Filter.Empty;
				var cursor = await AvatarCollection.FindAsync(filter);
				avatars = cursor.ToList().ToArray();
				Cache.Add(cacheKey, avatars, DateTimeOffset.Now.AddHours(1));
				log.Info("Populating the cache with avatars from database");

			} else {
				log.Info("Using a cached instance of avatars");
			}

			return avatars;
		}

		public static async Task<Color[]> GetAllColors (TraceWriter log) {
			const string cacheKey = "allColors";
			Color[] colors = Cache[cacheKey] as Color[];
			if (colors == null) {
				var filter = Builders<Color>.Filter.Empty;
				var cursor = await ColorCollection.FindAsync(filter);
				colors = cursor.ToList().ToArray();
				Cache.Add(cacheKey, colors, DateTimeOffset.Now.AddHours(1));
				log.Info("Populating the cache with colors from database");

			} else {
				log.Info("Using a cached instance of colors");
			}

			return colors;
		}

		public static async Task<Hat[]> GetAllHats (TraceWriter log) {
			const string cacheKey = "allHats";
			Hat[] hats = Cache[cacheKey] as Hat[];
			if (hats == null) {
				var filter = Builders<Hat>.Filter.Empty;
				var cursor = await HatCollection.FindAsync(filter);
				hats = cursor.ToList().ToArray();
				Cache.Add(cacheKey, hats, DateTimeOffset.Now.AddHours(1));
				log.Info("Populating the cache with hats from database");

			} else {
				log.Info("Using a cached instance of hats");
			}

			return hats;
		}

		public static Task CreateInventory(Inventory inventory) {
			return InventoryCollection.InsertOneAsync(inventory);
		}

		public static async Task<Inventory> GetInventory(Guid playerId) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerId);
			var cursor = await InventoryCollection.FindAsync(filter);
			Inventory inventory = cursor.Single();
			return inventory;
		}

		public static async Task AddAvatarToInventory(Guid playerid, int avatarid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedAvatars", avatarid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public static async Task AddColorToInventory(Guid playerid, int colorid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedColors", colorid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public static async Task AddHatToInventory(Guid playerid, int hatid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedHats", hatid);
			await InventoryCollection.UpdateOneAsync(filter, update);
		}

		public static async Task<bool> InventoryHasHat(Guid playerId, int hatId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId),
				Builders<Inventory>.Filter.Where(x => x.OwnedHats.Contains(hatId)));

			return (await InventoryCollection.FindAsync(filter)).Any();
		}

		public static async Task<bool> InventoryHasAvatar(Guid playerId, int avatarId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId),
				Builders<Inventory>.Filter.Where(x => x.OwnedAvatars.Contains(avatarId)));

			return (await InventoryCollection.FindAsync(filter)).Any();
		}

		public static async Task<bool> InventoryHasColor(Guid playerId, int colorId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId),
				Builders<Inventory>.Filter.Where(x => x.OwnedColors.Contains(colorId)));

			return (await InventoryCollection.FindAsync(filter)).Any();
		}

		public static async Task EquipHat(Guid playerId, int hatId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Hat", hatId);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task EquipAvatar(Guid playerId, int avatarId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Avatar", avatarId);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task EquipColor(Guid playerId, int colorId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Color", colorId);
			await PlayerCollection.UpdateOneAsync(filter, update);
		}

		public static async Task<Avatar> GetAvatar(int id, TraceWriter log) {
			var avatars = await GetAllAvatars(log);
			return avatars.Single(x => x.Id == id);
		}

		public static async Task<Color> GetColor(int id, TraceWriter log) {
			var colors = await GetAllColors(log);
			return colors.Single(x => x.Id == id);
		}

		public static async Task<Hat> GetHat(int id, TraceWriter log) {
			var hats = await GetAllHats(log);
			return hats.Single(x => x.Id == id);
		}


		public static async Task<PlayerStatsSingle> GetSingleStats(Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player.SingleStats;
		}


		public static async Task<PlayerStatsMulti> GetMultiStats(Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await PlayerCollection.FindAsync(filter);
			Player player = cursor.SingleOrDefault();
			return player.MultiStats;
		}

		public static async Task AddStatsSingle(Guid playerid, SessionStatsSingle stats) {
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

		public static async Task AddStatsMulti(Guid playerid, SessionStatsMulti stats) {
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
			string cacheKey = $"topSingle_{number}_{page}";
			IEnumerable<PlayerStatsView> topPlayers = Cache[cacheKey] as IEnumerable<PlayerStatsView>;
			if (topPlayers == null) {
				topPlayers = await DbGetTopPlayerStatsSingle(number, page);
				Cache.Set(cacheKey, topPlayers, DateTimeOffset.Now.AddMinutes(leaderboardsCacheMinutes));
			}

			return topPlayers;
		}


		private static async Task<IEnumerable<PlayerStatsView>> DbGetTopPlayerStatsSingle(int number, int page) {
			var sort = Builders<Player>.Sort.Descending(x => x.SingleStats.BestPoints);
			var aggregate = PlayerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.SingleStats.PlayedGames > 0)
				.Skip(page * number)
				.Limit(number)
				.Project(x => new PlayerStatsView()
				{
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
			string cacheKey = $"topMulti_{number}_{page}";
			IEnumerable<PlayerStatsView> topPlayers = Cache[cacheKey] as IEnumerable<PlayerStatsView>;
			if (topPlayers == null) {
				topPlayers = await DbGetTopPlayerStatsMulti(number, page);
				Cache.Set(cacheKey, topPlayers, DateTimeOffset.Now.AddMinutes(leaderboardsCacheMinutes));
			}

			return topPlayers;
		}

		private static async Task<IEnumerable<PlayerStatsView>> DbGetTopPlayerStatsMulti(int number, int page) {
			var sort = Builders<Player>.Sort.Descending(x => x.MultiStats.BestPoints);
			var aggregate = PlayerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.MultiStats.PlayedGames > 0)
				.Skip(page * number)
				.Limit(number)
				.Project(x => new PlayerStatsView()
				{
					Username = x.Username,
					Pizzeria = x.Pizzeria,
					Accuracy = x.MultiStats.Dropped == 0 ? 0 : (x.MultiStats.PinpointAccuracy / (float)x.MultiStats.Dropped),
					AllPoints = x.MultiStats.AllPoints,
					BestPoints = x.MultiStats.BestPoints,
					Distance = x.MultiStats.Distance,
					PizzasDelivered = x.MultiStats.Hits,
					PlayedGames = x.MultiStats.PlayedGames,
					PlayTime = x.MultiStats.OverallGameTime
				});
			return await aggregate.ToListAsync();
		}

		public static Task CreatePlayerToken(PlayerAuthorizationToken token) {
			var filter = Builders<PlayerAuthorizationToken>.Filter.Eq("PlayerId", token.PlayerId);
			return TokenCollection.ReplaceOneAsync(filter, token, new UpdateOptions() {IsUpsert = true });
		}

		public static async Task<PlayerAuthorizationToken> GetPlayerToken(Guid playerId) {
			var filter = Builders<PlayerAuthorizationToken>.Filter.Eq("PlayerId", playerId);
			var cursor = await TokenCollection.FindAsync(filter);
			PlayerAuthorizationToken token = cursor.Single();
			return token;
		}

		public static async Task RemoveFakeUsers() {
			var filter = Builders<Player>.Filter.Regex("Username", "FakeUser[0-9]*");

			var ids = (await PlayerCollection.Aggregate().Match(filter).Project(x => new {id = x.Id}).ToListAsync()).Select(x => x.id);

			await PlayerCollection.DeleteManyAsync(filter);

			var deletefilter = Builders<Inventory>.Filter.In("PlayerId", ids);
			await InventoryCollection.DeleteManyAsync(deletefilter);
			ClearCache();
		}

		public static async Task RemoveStrayInventories(TraceWriter log) {
			var pIds = (await PlayerCollection.Find(Builders<Player>.Filter.Empty).Project(x=> new{id = x.Id}).ToListAsync()).Select(x => x.id);
			var iIds = (await InventoryCollection.Find(Builders<Inventory>.Filter.Empty).Project(x => new { id = x.PlayerId }).ToListAsync()).Select(x => x.id);

			var strays = iIds.Except(pIds).ToList();

			foreach (Guid stray in strays) {
				log.Info("Stray: "+stray.ToString());
			}

			var filter = Builders<Inventory>.Filter.In("PlayerId", strays);
			await InventoryCollection.DeleteManyAsync(filter);
		}

		public static void ClearCache() {
			List<string> cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
			foreach (string cacheKey in cacheKeys) {
				Cache.Remove(cacheKey);
			}
		}
	}
}
