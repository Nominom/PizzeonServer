using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server
{
    public interface IRepository
    {
        Task CreatePlayer (Player player);
        Task <bool> CheckUsernameAvailable(string username);
        Task RemovePlayer (Guid id);
        Task<Player> GetPlayer(Guid id);
        Task CreateHat(Hat hat);
        Task RemoveHat(Guid id);
        Task CreateColor(Color color);
        Task<PlayerStatsSingle> GetSingleStats(Guid playerid);
        Task<PlayerStatsMulti> GetMultiStats(Guid playerid);
        Task RemoveColor(Guid id);
        Task CreateAvatar(Avatar avatar);
        Task RemoveAvatar(Guid id);
        Task CreateInventory (Inventory inventory);
        Task RemoveInventory (Guid id);
        Task AddStatsSingle(Guid playerid, SessionStatsSingle stats);
        Task AddStatsMulti(Guid playerid, SessionStatsMulti stats);
        Task AddHatToInventory (Guid playerid, string hatid);
        Task AddAvatarToInventory (Guid playerid, string avatarid);
        Task AddColorToInventory (Guid playerid, string colorid);
        Task<Inventory> GetInventory (Guid playerId);
        Task<Hat> GetHat (string id);
        Task<Avatar> GetAvatar (string id); 
        Task<Color> GetColor (string id);
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
	    Task<IEnumerable<PlayerStatsView>> GetTopPlayerStatsSingle (int number, int page);
	    Task<IEnumerable<PlayerStatsView>> GetTopPlayerStatsMulti (int number, int page);
	}
}