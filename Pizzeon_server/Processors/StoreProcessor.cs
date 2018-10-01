using System;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server.Processors
{
    public class StoreProcessor
    {
        private IRepository _repository;

        private PlayerProcessor _playerProcessor;

        private InventoryProcessor _inventoryProcessor;

        public StoreProcessor (IRepository repository, PlayerProcessor playerProcessor, InventoryProcessor inventoryProcessor) {
            _repository = repository;
            _playerProcessor = playerProcessor;
            _inventoryProcessor = inventoryProcessor;
        }

        public void CreateColor(NewColor newColor) {
            Color color = new Color();
            color.Name = newColor.Name;
	        color.Id = newColor.Id;
            color.Price = newColor.Price;
            _repository.CreateColor(color);
        }

        public void CreateAvatar(NewAvatar newAvatar) {
            Avatar avatar = new Avatar();
            avatar.Name = newAvatar.Name;
	        avatar.Id = newAvatar.Id;
            avatar.Price = newAvatar.Price;
            _repository.CreateAvatar(avatar);
        }

        public void CreateHat(NewHat newHat) {
            Hat hat = new Hat();
            hat.Name = newHat.Name;
	        hat.Id = newHat.Id;
            hat.Description = newHat.Description;
            hat.Price = newHat.Price;
            _repository.CreateHat(hat);

        }

        public Task<Hat> GetHat(string Id) {
            return _repository.GetHat(Id);
        }

        public Task<Avatar> GetAvatar(string Id) {
            return _repository.GetAvatar(Id);
        }

        public Task<Color> GetColor(string Id) {
            return _repository.GetColor(Id);
        }

		public Task<Hat[]> GetAllHats() {
			return _repository.GetAllHats();
		}

		public Task<Color[]> GetAllColors() {
			return _repository.GetAllColors();
		}

		public Task<Avatar[]> GetAllAvatars() {
			return _repository.GetAllAvatars();
		}

        public bool BuyHat(Guid playerId, string hatId) {
            Player player = _repository.GetPlayer(playerId).Result;
            Hat hat = GetHat(hatId).Result;
            if (player.Money >= hat.Price) {
                _playerProcessor.DeductCoin(playerId, hat.Price);
                _inventoryProcessor.AddHatToInventory(playerId, hatId);
	            return true;
            }
            else {
//      TODO: Show error message
	            return false;
            }
        }

        public void BuyAvatar(Guid playerId, string avatarId) {
            Player player = _repository.GetPlayer(playerId).Result;
            Avatar avatar = GetAvatar(avatarId).Result;
            if (player.Money >= avatar.Price) {
                _playerProcessor.DeductCoin(playerId, avatar.Price);
                _inventoryProcessor.AddAvatarToInventory(playerId, avatarId);
            }
            else {
//      TODO: Show error message
                return;                
            }
        }

        public void BuyColor(Guid playerId, string colorId) {
            Player player = _repository.GetPlayer(playerId).Result;
            Color color = GetColor(colorId).Result;
            if (player.Money >= color.Price) {
                _playerProcessor.DeductCoin(playerId, color.Price);
                _inventoryProcessor.AddColorToInventory(playerId, colorId);
            }
            else {
//      TODO: Show error message
                return;                
            }
        }

        public void DeleteColor(Guid Id) {
            _repository.RemoveColor(Id);
        }

        public void DeleteAvatar(Guid Id) {
            _repository.RemoveAvatar(Id);
        }

        public void DeleteHat(Guid Id) {
            _repository.RemoveHat(Id);
        }
    }
}