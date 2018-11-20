using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pizzeon_server.Models;

namespace Pizzeon_server {
	public class MongoDbRepository : IRepository {

		private MongoClient _client;
		private IMongoDatabase _database;
		private IMongoCollection<Player> _playerCollection;
		private IMongoCollection<Inventory> _inventoryCollection;
		private IMongoCollection<Hat> _hatCollection;
		private IMongoCollection<Color> _colorCollection;
		private IMongoCollection<Avatar> _avatarCollection;


		public MongoDbRepository (IConfiguration conf) {
			string connectionString = conf["mongodb_connection"];
			_client = new MongoClient(connectionString);
			_database = _client.GetDatabase("pizzeon");
			_playerCollection = _database.GetCollection<Player>("players");
			_inventoryCollection = _database.GetCollection<Inventory>("inventories");
			_hatCollection = _database.GetCollection<Hat>("hats");
			_colorCollection = _database.GetCollection<Color>("colors");
			_avatarCollection = _database.GetCollection<Avatar>("avatars");

		}

		public async Task AddAvatarToInventory (Guid playerid, string avatarid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedAvatars", avatarid);
			await _inventoryCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddColorToInventory (Guid playerid, string colorid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedColors", colorid);
			await _inventoryCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddHatToInventory (Guid playerid, string hatid) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerid);
			var update = Builders<Inventory>.Update.AddToSet("OwnedHats", hatid);
			await _inventoryCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddStatsMulti (Guid playerid, SessionStatsMulti stats) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await _playerCollection.FindAsync(filter);
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
			await _playerCollection.UpdateOneAsync(filter, update);

		}

		public async Task AddStatsSingle (Guid playerid, SessionStatsSingle stats) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await _playerCollection.FindAsync(filter);
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
			await _playerCollection.UpdateOneAsync(filter, update);
		}

		public async Task<bool> CheckUsernameAvailable (string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await _playerCollection.FindAsync(filter);
			if (cursor.Any()) {
				return false;
			}

			return true;
		}

		public async Task CreateAvatar (Avatar avatar) {
			var filter = Builders<Avatar>.Filter.Eq("Id", avatar.Id);
			var existingAvatar = await _avatarCollection.FindAsync(filter);

			if (!existingAvatar.Any()) {
				await _avatarCollection.InsertOneAsync(avatar);
			}
			else {
				await _avatarCollection.ReplaceOneAsync(filter, avatar);
			}
		}

		public async Task CreateColor (Color color) {
			var filter = Builders<Color>.Filter.Eq("Id", color.Id);
			var existingColor = await _colorCollection.FindAsync(filter);

			if (!existingColor.Any()) {
				await _colorCollection.InsertOneAsync(color);
			}
			else {
				await _colorCollection.ReplaceOneAsync(filter, color);
			}
		}

		public async Task CreateHat (Hat hat) {
			var filter = Builders<Hat>.Filter.Eq("Id", hat.Id);
			var existingHat = await _hatCollection.FindAsync(filter);

			if (!existingHat.Any()) {
				await _hatCollection.InsertOneAsync(hat);
			}
			else {
				await _hatCollection.ReplaceOneAsync(filter, hat);
			}
		}

		public Task CreateInventory (Inventory inventory) {
			return _inventoryCollection.InsertOneAsync(inventory);
		}

		public Task CreatePlayer (Player player) {
			return _playerCollection.InsertOneAsync(player);
		}

		public async Task DeductCoinFromPlayer (Guid playerId, int price) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", -price);
			await _playerCollection.UpdateOneAsync(filter, update);
		}

		public async Task AddCoinToPlayer (Guid playerId, int coin) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Inc("Money", +coin);
			await _playerCollection.UpdateOneAsync(filter, update);
		}

		public async Task<Avatar[]> GetAllAvatars() {
			Console.WriteLine("Inside MongoDBRepository, getting avatars...");
			var filter = Builders<Avatar>.Filter.Empty;
			var cursor = await _avatarCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public async Task<Color[]> GetAllColors() {
			var filter = Builders<Color>.Filter.Empty;
			var cursor = await _colorCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public async Task<Hat[]> GetAllHats() {
			var filter = Builders<Hat>.Filter.Empty;
			var cursor = await _hatCollection.FindAsync(filter);
			return cursor.ToList().ToArray();
		}

		public async Task<Avatar> GetAvatar (string id) {
			var filter = Builders<Avatar>.Filter.Eq("Id", id);
			var cursor = await _avatarCollection.FindAsync(filter);
			Avatar avatar = cursor.Single();
			return avatar;
		}

		public async Task<Color> GetColor (string id) {
			var filter = Builders<Color>.Filter.Eq("Id", id);
			var cursor = await _colorCollection.FindAsync(filter);
			Color color = cursor.Single();
			return color;
		}

		public async Task<Hat> GetHat (string id) {
			var filter = Builders<Hat>.Filter.Eq("Id", id);
			var cursor = await _hatCollection.FindAsync(filter);
			Hat hat = cursor.Single();
			return hat;
		}

		public async Task<Inventory> GetInventory (Guid playerId) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", playerId);
			var cursor = await _inventoryCollection.FindAsync(filter);
			Inventory inventory = cursor.Single();
			return inventory;
		}

		public async Task<PlayerStatsMulti> GetMultiStats (Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await _playerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player.MultiStats;
		}

		public async Task<Player> GetPlayer (Guid id) {
			var filter = Builders<Player>.Filter.Eq("Id", id);
			var cursor = await _playerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player;
		}

		public async Task<Player> GetPlayerByName (string username) {
			var filter = Builders<Player>.Filter.Eq("Username", username);
			var cursor = await _playerCollection.FindAsync(filter);
			Player player = cursor.SingleOrDefault();
			return player;
		}

		public async Task<bool> InventoryHasHat(Guid playerId, string hatId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId), 
				Builders<Inventory>.Filter.Where(x => x.OwnedHats.Contains(hatId))) ;

			return (await _inventoryCollection.FindAsync(filter)).Any();
		}

		public async Task<bool> InventoryHasAvatar(Guid playerId, string avatarId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId), 
				Builders<Inventory>.Filter.Where(x => x.OwnedAvatars.Contains(avatarId)));

			return (await _inventoryCollection.FindAsync(filter)).Any();
		}

		public async Task<bool> InventoryHasColor(Guid playerId, string colorId) {
			var filter = Builders<Inventory>.Filter.And(
				Builders<Inventory>.Filter.Eq("PlayerId", playerId), 
				Builders<Inventory>.Filter.Where(x => x.OwnedColors.Contains(colorId)));

			return (await _inventoryCollection.FindAsync(filter)).Any();
		}

		public async Task EquipHat(Guid playerId, string hatId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Hat", hatId);
			await _playerCollection.UpdateOneAsync(filter, update);
		}

		public async Task EquipAvatar(Guid playerId, string avatarId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Avatar", avatarId);
			await _playerCollection.UpdateOneAsync(filter, update);
		}

		public async Task EquipColor(Guid playerId, string colorId) {
			var filter = Builders<Player>.Filter.Eq("Id", playerId);
			var update = Builders<Player>.Update.Set("Color", colorId);
			await _playerCollection.UpdateOneAsync(filter, update);
		}

		public async Task<IEnumerable<PlayerStatsView>> GetAllPlayerStatsSingle() {
			var sort = Builders<Player>.Sort.Descending(x => x.SingleStats.BestPoints);
			var aggregate = _playerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.SingleStats.PlayedGames > 0)
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

		public async Task<IEnumerable<PlayerStatsView>> GetTopPlayerStatsSingle(int number, int page) {
			var sort = Builders<Player>.Sort.Descending(x => x.SingleStats.BestPoints);
			var aggregate = _playerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.SingleStats.PlayedGames > 0)
				.Skip(page * number)
				.Limit(number)
				.Project(x => new PlayerStatsView() {
					Username = x.Username,
					Pizzeria = x.Pizzeria,
					Accuracy = x.SingleStats.Dropped == 0? 0 : (x.SingleStats.PinpointAccuracy/(float)x.SingleStats.Dropped),
					AllPoints = x.SingleStats.AllPoints,
					BestPoints = x.SingleStats.BestPoints,
					Distance = x.SingleStats.Distance,
					PizzasDelivered = x.SingleStats.Hits,
					PlayedGames = x.SingleStats.PlayedGames,
					PlayTime = x.SingleStats.OverallGameTime
				});
			return await aggregate.ToListAsync();
		}

		public async Task<IEnumerable<PlayerStatsView>> GetAllPlayerStatsMulti() {
			var sort = Builders<Player>.Sort.Descending(x => x.MultiStats.BestPoints);
			var aggregate = _playerCollection.Aggregate()
				.Sort(sort)
				.Match(x => x.MultiStats.PlayedGames > 0)
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

		public async Task<IEnumerable<PlayerStatsView>> GetTopPlayerStatsMulti(int number, int page) {
			var sort = Builders<Player>.Sort.Descending(x => x.MultiStats.BestPoints);
			var aggregate = _playerCollection.Aggregate()
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

		public async Task<PlayerStatsSingle> GetSingleStats (Guid playerid) {
			var filter = Builders<Player>.Filter.Eq("Id", playerid);
			var cursor = await _playerCollection.FindAsync(filter);
			Player player = cursor.Single();
			return player.SingleStats;
		}

		public async Task RemoveAvatar (Guid id) {
			var filter = Builders<Avatar>.Filter.Eq("Id", id);
			await _avatarCollection.DeleteOneAsync(filter);
		}

		public async Task RemoveColor (Guid id) {
			var filter = Builders<Color>.Filter.Eq("Id", id);
			await _colorCollection.DeleteOneAsync(filter);
		}

		public async Task RemoveHat (Guid id) {
			var filter = Builders<Hat>.Filter.Eq("Id", id);
			await _hatCollection.DeleteOneAsync(filter);
		}

		public async Task RemoveInventory (Guid id) {
			var filter = Builders<Inventory>.Filter.Eq("PlayerId", id);
			await _inventoryCollection.DeleteOneAsync(filter);
		}

		public async Task RemovePlayer (Guid id) {
			var filter = Builders<Player>.Filter.Eq("Id", id);
			await _playerCollection.DeleteOneAsync(filter);
		}
	}
}
