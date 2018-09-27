using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Processors
{
    public class StatsProcessor
    {
        private IRepository _repository;

        public StatsProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Stat[]> GetStats(Guid playerid)
        {
            return _repository.GetStats(playerid);
        }

        public void AddStats(Guid playerid, SessionStats stats)
        {
            return _repository.AddStats(playerid, stats);
        }
    }
}
