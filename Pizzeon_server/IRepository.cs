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
    }
}