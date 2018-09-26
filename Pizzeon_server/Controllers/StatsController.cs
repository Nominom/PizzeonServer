using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        
        [HttpGet("{Id:Guid}")]
        public Task<Stat[]> GetStats(Guid playerid)
        {
            return _processor.GetStats(playerid);
        }

        [HttpPost]
        public void AddStats(Guid playerid, SessionStats stats)
        {
            
        }
    }
}
