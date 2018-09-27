using System;
using Pizzeon_server.Models;

namespace Pizzeon_server.Processors
{
    public class PlayerProcessor {
        
        private IRepository _repository;

        public PlayerProcessor (IRepository repository) 
        {
            _repository = repository;
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
            player.Stats = new PlayerStats();
            _repository.CreatePlayer(player);
        }

        public void DeletePlayer (Guid Id) 
        {
            _repository.RemovePlayer(Id);
        }
    }
}