using System;
using Pizzeon_server.Models;

namespace Pizzeon_server.Processors
{
    public class StoreProcessor
    {
        private IRepository _repository;

        public StoreProcessor (IRepository repository) {
            _repository = repository;
        }

        public void CreateColor(NewColor newColor) {
            Color color = new Color();
            color.Name = newColor.Name;
            color.Id = Guid.NewGuid();
            color.Price = newColor.Price;
            _repository.CreateColor(color);
        }

        public void CreateAvatar(NewAvatar newAvatar) {
            Avatar avatar = new Avatar();
            avatar.Name = newAvatar.Name;
            avatar.Id = Guid.NewGuid();
            avatar.Price = newAvatar.Price;
            _repository.CreateAvatar(avatar);
        }

        public void CreateHat(NewHat newHat) {
            Hat hat = new Hat();
            hat.Name = newHat.Name;
            hat.Id = Guid.NewGuid();
            hat.Description = newHat.Description;
            hat.Price = newHat.Price;
            _repository.CreateHat(hat);

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