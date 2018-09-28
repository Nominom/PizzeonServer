using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pizzeon_server.Models;
using Pizzeon_server.Processors;

namespace Pizzeon_server.Controllers
{
	[Route("api/players")]
	[ApiController]
	public class PlayerController : ControllerBase {

        private PlayerProcessor _playerProcessor;

        public PlayerController(PlayerProcessor playerProcessor) {
            _playerProcessor = playerProcessor;
        }

		// POST api/values
		[HttpPost]
		public ActionResult CreatePlayer ([FromBody] NewPlayer newPlayer) {
			if (_playerProcessor.CreatePlayer(newPlayer)) {
            	return Ok();
			} else {
				return StatusCode(StatusCodes.Status409Conflict, "Username already taken!");
			}
        }
		[HttpPost("login")]
		public ActionResult <Guid> Login ([FromBody] LoginCredentials credentials) {
			Guid guid = _playerProcessor.Login(credentials);
			if (guid == Guid.Empty) {
				return StatusCode(StatusCodes.Status401Unauthorized, Guid.Empty);
			} else {
				return StatusCode(StatusCodes.Status200OK, guid);
			}
		}

		// DELETE api/values/5
		[HttpDelete("{playerid}")]
		public ActionResult DeletePlayer (Guid playerid) {
            _playerProcessor.DeletePlayer(playerid);
            return Ok();
		}
	}
}