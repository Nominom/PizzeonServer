using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server.Processors
{
    public class StatsProcessor
    {
        private IRepository _repository;

        public StatsProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<PlayerStats> GetStats(Guid playerid)
        {
            return _repository.GetStats(playerid);
        }

        public void AddStats(Guid playerid, SessionStats stats)
        {
            _repository.AddStats(playerid, stats);
        }
    }
}
