using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeon_server.Models;
using Pizzeon_server.Processors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pizzeon_server.Controllers {
	[Route("api/scoreboard")]
	public class ScoreboardsController : Controller {

		private ScoreboardProcessor _scoreboardProcessor;

		public ScoreboardsController (ScoreboardProcessor scoreboardProcessor) {
			_scoreboardProcessor = scoreboardProcessor;
		}

		// GET: api/scoreboard/single?number=10;page=0
		[HttpGet("single")]
		public IEnumerable<PlayerStatsView> GetSingle ([FromQuery] int? number, [FromQuery] int? page) {
			int num = number ?? 100;
			int pag = page ?? 0;
			return _scoreboardProcessor.GetTopPlayerStatsSingle(num, pag);
		}

		// GET: api/scoreboard/multi?number=10;page=0
		[HttpGet("multi")]
		public IEnumerable<PlayerStatsView> GetMulti ([FromQuery] int? number, [FromQuery] int? page) {
			int num = number ?? 100;
			int pag = page ?? 0;
			return _scoreboardProcessor.GetTopPlayerStatsMulti(num, pag);
		}
	}
}
