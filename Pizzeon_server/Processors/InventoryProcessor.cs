using System;
using Pizzeon_server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pizzeon_server.Processors
{
    public class InventoryProcessor
    {
        private IRepository _repository;

        public InventoryProcessor (IRepository repository) 
        {
            _repository = repository;
        }

        public void CreateInventory (Guid PlayerId) 
        {
            Inventory inventory = new Inventory();
            inventory.PlayerId = PlayerId;
            inventory.OwnedAvatars = new List<Guid>();
            inventory.OwnedColors = new List<Guid>();
            inventory.OwnedHats = new List<Guid>();
            _repository.CreateInventory(inventory);
        }

        public void AddHatToInventory (Guid PlayerId, Guid HatId) 
        {
            _repository.AddHatToInventory(PlayerId, HatId);
        }

        public void AddColorToInventory (Guid PlayerId, Guid ColorId) 
        {
            _repository.AddColorToInventory(PlayerId, ColorId);
        }

        public void AddAvatarToInventory (Guid PlayerId, Guid AvatarId) 
        {
            _repository.AddAvatarToInventory(PlayerId, AvatarId);
        }

        public Task<Inventory> GetInventory (Guid PlayerId) 
        {
            return _repository.GetInventory(PlayerId);
        }

        public void RemoveInventory (Guid PlayerId) 
        {
            _repository.RemoveInventory(PlayerId);
        }
    }
}