using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeon_server.Models;
using Pizzeon_server.Processors;

namespace Pizzeon_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase {

        private PlayerProcessor _playerProcessor;

        public PlayerController(PlayerProcessor playerProcessor) {
            _playerProcessor = playerProcessor;
        }

		// POST api/values
		[HttpPost]
		public ActionResult CreatePlayer ([FromBody] NewPlayer newPlayer) {
		    _playerProcessor.CreatePlayer(newPlayer);
            return Ok();
        }

		// DELETE api/values/5
		[HttpDelete("{playerid}")]
		public ActionResult DeletePlayer (Guid playerid) {
            _playerProcessor.DeletePlayer(playerid);
            return Ok();
		}
	}
}