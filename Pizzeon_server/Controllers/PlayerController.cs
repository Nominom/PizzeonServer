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
		public JsonResult Login ([FromBody] LoginCredentials credentials) {
			Guid guid = _playerProcessor.Login(credentials);
			if (guid == Guid.Empty) {
				JsonResult result = new JsonResult(Guid.Empty);
				result.StatusCode = StatusCodes.Status401Unauthorized;

				return result;
			} else {
				JsonResult result = new JsonResult(guid);
				result.StatusCode = StatusCodes.Status200OK;

				return result;
			}
		}

		[HttpGet("{playerid}")]
		public JsonResult GetInfo(Guid playerid) {
			PlayerInfo info = _playerProcessor.GetInfo(playerid);
			if (info == null) {
				JsonResult result = new JsonResult("No player was found with this ID.");
				result.StatusCode = StatusCodes.Status404NotFound;

				return result;
			}
			else {
				JsonResult result = new JsonResult(info);
				result.StatusCode = StatusCodes.Status200OK;

				return result;
			}
		}

		// DELETE api/values/5
		[HttpDelete("{playerid}")]
		[AdminAuth]
		public ActionResult DeletePlayer (Guid playerid) {
            _playerProcessor.DeletePlayer(playerid);
            return Ok();
		}
	}
}