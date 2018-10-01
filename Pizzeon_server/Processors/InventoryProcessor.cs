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
				return false;
			}
		}

		public void AddHatToInventory (Guid PlayerId, string HatId) 
        {
            _repository.AddHatToInventory(PlayerId, HatId);
        }

        public void AddColorToInventory (Guid PlayerId, string ColorId) 
        {
            _repository.AddColorToInventory(PlayerId, ColorId);
        }

        public void AddAvatarToInventory (Guid PlayerId, string AvatarId) 
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