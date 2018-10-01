using System;
using System.Linq;
using Pizzeon_server.Models;

namespace Pizzeon_server.Processors
{
    public class PlayerProcessor {

        private InventoryProcessor _inventoryProcessor;
        
        private IRepository _repository;

        public PlayerProcessor (IRepository repository, InventoryProcessor inventoryProcessor) 
        {
            _repository = repository;
            _inventoryProcessor = inventoryProcessor;
        }

        public bool CreatePlayer (NewPlayer newPlayer) 
        {
            if (!_repository.CheckUsernameAvailable(newPlayer.Username).Result) {
                return false;
            }

            Player player = new Player();
            player.Username = newPlayer.Username;
            player.Password = newPlayer.Password;
            player.Id = Guid.NewGuid();
            player.CreationTime = System.DateTime.Now;
            player.Hat = string.Empty;
            player.Color = string.Empty;
            player.Avatar = string.Empty;
            player.Money = 0;
	        string pizzeriaUsername = player.Username.First().ToString().ToUpper() + player.Username.Substring(1);
            player.Pizzeria = pizzeriaUsername + "'s Pizzeria";
            player.SingleStats = new PlayerStatsSingle();
            player.MultiStats = new PlayerStatsMulti();
            _repository.CreatePlayer(player);
            _inventoryProcessor.CreateInventory(player.Id);

            return true;
        }

        public PlayerAuthorizationToken Login(LoginCredentials credentials)
        {
	        try {
		        Player player = _repository.GetPlayerByName(credentials.Username).Result;
		        if (player.Password == credentials.Password) {
			        var token = PlayerAuthorizationToken.Create(player.Id);
					PlayerAuthKeyStorage.AddToken(token);
					return token;
		        }
		        else {
			        return null;
		        }
	        }
	        catch (Exception) {
				return null;
	        }
		}

        public void DeletePlayer (Guid Id) 
        {
            _repository.RemovePlayer(Id);
            _inventoryProcessor.RemoveInventory(Id);
        }

        public void DeductCoin(Guid playerId, int price)
        {
            _repository.DeductCoinFromPlayer(playerId, price);
        }

	    public PlayerInfo GetInfo(Guid playerid) {
		    try {
			    var player = _repository.GetPlayer(playerid).Result;
			    PlayerInfo info = new PlayerInfo();
			    info.Id = player.Id;
				info.Username = player.Username;
			    info.Avatar = player.Avatar;
			    info.Hat = player.Hat;
			    info.Color = player.Color;
			    info.Money = player.Money;
			    info.Pizzeria = player.Pizzeria;
			    return info;
		    }
		    catch (Exception) {
			    return null;
		    }

	    }
    }
}