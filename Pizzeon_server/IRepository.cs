using System;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server
{
    public interface IRepository
    {
        Task AddPlayer (Player player);
        Task RemovePlayer (Guid Id);
        Task <Player> GetPlayer(Guid Id);
        Task<PlayerStats> GetStats(Guid playerid);
        Task AddStats(Guid playerid, SessionStats stats);
    }
}