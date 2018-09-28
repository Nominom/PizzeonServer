using System;
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

        public void CreatePlayer (NewPlayer newPlayer) 
        {
            Player player = new Player();
            player.Username = newPlayer.Username;
            player.Id = Guid.NewGuid();
            player.CreationTime = System.DateTime.Now;
            player.Hat = Guid.Empty;
            player.Color = Guid.Empty;
            player.Avatar = Guid.Empty;
            player.Coin = 0;
            player.Pizzeria = player.Username + "'s Pizzeria";
            player.SingleStats = new PlayerStatsSingle();
            player.MultiStats = new PlayerStatsMulti();
            _repository.CreatePlayer(player);
            _inventoryProcessor.CreateInventory(player.Id);
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
    }
}