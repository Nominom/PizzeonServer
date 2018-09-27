using System;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server
{
    public interface IRepository
    {
        Task CreatePlayer (Player player);
        Task RemovePlayer (Guid Id);
        Task <Player> GetPlayer(Guid Id);
        Task<PlayerStats> GetStats(Guid playerid);
        Task AddStats(Guid playerid, SessionStats stats);
        Task CreateHat(Hat hat);
        Task RemoveHat(Guid Id);
        Task CreateColor(Color color);
        Task RemoveColor(Guid Id);
        Task CreateAvatar(Avatar avatar);
        Task RemoveAvatar(Guid Id);
    }
}