using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace PizzeonAzureFunctions {
	public static class InternalFunctions {


		public static async Task CreatePlayerInventory(Guid playerId) {

			Inventory inventory = new Inventory();
			inventory.PlayerId = playerId;
			inventory.OwnedAvatars = new List<string>();
			inventory.OwnedColors = new List<string>();
			inventory.OwnedHats = new List<string>();

			inventory.OwnedAvatars.Add("0");
			inventory.OwnedColors.Add("0");
			inventory.OwnedHats.Add("0");

			await MongoDbRepository.CreateInventory(inventory);
		}
	}
}
