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

        public void CreateInventory (Guid playerId) 
        {
            Inventory inventory = new Inventory();
            inventory.PlayerId = playerId;
            inventory.OwnedAvatars = new List<string>();
            inventory.OwnedColors = new List<string>();
            inventory.OwnedHats = new List<string>();
            _repository.CreateInventory(inventory);
        }

	    public bool EquipHat(Guid playerId, string hatId) {
		    try {
			    if (_repository.InventoryHasHat(playerId, hatId).Result) {
				    _repository.EquipHat(playerId, hatId);
				    return true;
			    }
			    else {
				    return false;
				}
		    }
		    catch (Exception ex) {
			    Console.WriteLine(ex);
				return false;
		    }
		}

	    public bool EquipAvatar (Guid playerId, string avatarId) {
			try {
				if (_repository.InventoryHasAvatar(playerId, avatarId).Result) {
					_repository.EquipAvatar(playerId, avatarId);
					return true;
				} else {
					return false;
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
				return false;
			}
		}

	    public bool EquipColor (Guid playerId, string colorId) {
			try {
				if (_repository.InventoryHasColor(playerId, colorId).Result) {
					_repository.EquipColor(playerId, colorId);
					return true;
				} else {
					return false;
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
				return false;
			}
		}

		public void AddHatToInventory (Guid playerId, string hatId) 
        {
            _repository.AddHatToInventory(playerId, hatId);
        }

        public void AddColorToInventory (Guid playerId, string colorId) 
        {
            _repository.AddColorToInventory(playerId, colorId);
        }

        public void AddAvatarToInventory (Guid playerId, string avatarId) 
        {
            _repository.AddAvatarToInventory(playerId, avatarId);
        }

        public Task<Inventory> GetInventory (Guid playerId) 
        {
            return _repository.GetInventory(playerId);
        }

        public void RemoveInventory (Guid playerId) 
        {
            _repository.RemoveInventory(playerId);
		}
	}
}