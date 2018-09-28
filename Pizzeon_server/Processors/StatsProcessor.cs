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

        public Task<PlayerStatsSingle> GetSingleStats(Guid playerid)
        {
            return _repository.GetSingleStats(playerid);
        }

         public Task<PlayerStatsMulti> GetMultiStats(Guid playerid)
        {
            return _repository.GetMultiStats(playerid);
        }

        public void AddStatsSingle(Guid playerid, SessionStatsSingle stats)
        {
            _repository.AddStatsSingle(playerid, stats);
        }

        public void AddStatsMulti(Guid playerid, SessionStatsMulti stats)
        {
            _repository.AddStatsMulti(playerid, stats);
        }
    }
}
