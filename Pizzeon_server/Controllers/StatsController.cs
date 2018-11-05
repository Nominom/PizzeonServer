using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeon_server.Models;
using Pizzeon_server.Processors;

namespace Pizzeon_server.Controllers
{
    [Route("api/players/{playerId:Guid}/stats")]
    [ApiController]
    public class StatsController
    {
        StatsProcessor _processor;

        public StatsController(StatsProcessor processor)
        {
            _processor = processor;
        }
        
        [HttpGet("single")]
        public Task<PlayerStatsSingle> GetSingleStats(Guid playerid)
        {
            return _processor.GetSingleStats(playerid);
        }

        [HttpGet("multi")]
        public Task<PlayerStatsMulti> GetMultiStats(Guid playerid)
        {
            return _processor.GetMultiStats(playerid);
        }

        [HttpPost("single")]
		[PlayerAuth("playerId")]
        public void AddStatsSingle(Guid playerId, [FromBody]SessionStatsSingle stats)
        {
            _processor.AddStatsSingle(playerId, stats);
        }

        [HttpPost("multi")]
        [PlayerAuth("playerId")]
		public void AddStatsMulti(Guid playerId, [FromBody]SessionStatsMulti stats)
        {
            _processor.AddStatsMulti(playerId, stats);
        }
    }
}
