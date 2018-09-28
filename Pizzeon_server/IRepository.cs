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
        Task AddHatToInventory (Guid playerid, Guid hatid);
        Task AddAvatarToInventory (Guid playerid, Guid avatarid);
        Task AddColorToInventory (Guid playerid, Guid colorid);
        Task<Inventory> GetInventory (Guid playerId);
        Task<Hat> GetHat (Guid Id);
        Task<Avatar> GetAvatar (Guid Id); 
        Task<Color> GetColor (Guid Id);
        Task DeductCoinFromPlayer(Guid playerId, int price);
        Task <Player> GetPlayerByName(string username);
    }
}