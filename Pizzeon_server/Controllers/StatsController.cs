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
		[PlayerAuth("playerid")]
        public void AddStatsSingle(Guid playerid, [FromBody]SessionStatsSingle stats)
        {
            _processor.AddStatsSingle(playerid, stats);
        }

        [HttpPost("multi")]
        [PlayerAuth("playerid")]
		public void AddStatsMulti(Guid playerid, [FromBody]SessionStatsMulti stats)
        {
            _processor.AddStatsMulti(playerid, stats);
        }
    }
}
