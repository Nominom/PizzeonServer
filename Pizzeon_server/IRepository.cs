using System;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server
{
    public interface IRepository
    {
        Task CreatePlayer (Player player);
        Task <bool> CheckUsernameAvailable(string username);
        Task RemovePlayer (Guid Id);
        Task<Player> GetPlayer(Guid Id);
        Task CreateHat(Hat hat);
        Task RemoveHat(Guid Id);
        Task CreateColor(Color color);
        Task<PlayerStatsSingle> GetSingleStats(Guid playerid);
        Task<PlayerStatsMulti> GetMultiStats(Guid playerid);
        Task RemoveColor(Guid Id);
        Task CreateAvatar(Avatar avatar);
        Task RemoveAvatar(Guid Id);
        Task CreateInventory (Inventory inventory);
        Task RemoveInventory (Guid Id);
        Task AddStatsSingle(Guid playerid, SessionStatsSingle stats);
        Task AddStatsMulti(Guid playerid, SessionStatsMulti stats);
        Task AddHatToInventory (Guid playerid, string hatid);
        Task AddAvatarToInventory (Guid playerid, string avatarid);
        Task AddColorToInventory (Guid playerid, string colorid);
        Task<Inventory> GetInventory (Guid playerId);
        Task<Hat> GetHat (string Id);
        Task<Avatar> GetAvatar (string Id); 
        Task<Color> GetColor (string Id);
        Task DeductCoinFromPlayer(Guid playerId, int price);
        Task <Player> GetPlayerByName(string username);
		Task<Hat[]> GetAllHats();
		Task<Color[]> GetAllColors();
		Task<Avatar[]> GetAllAvatars();
	    Task<bool> InventoryHasHat(Guid playerId, string hatId);
	    Task<bool> InventoryHasAvatar (Guid playerId, string avatarId);
	    Task<bool> InventoryHasColor (Guid playerId, string colorId);
	    Task EquipHat(Guid playerId, string hatId);
	    Task EquipAvatar (Guid playerId, string avatarId);
	    Task EquipColor (Guid playerId, string colorId);
	}
}