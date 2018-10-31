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
    [Route("api/store")]
	[ApiController]
    public class StoreController : ControllerBase
    {
        private StoreProcessor _processor;
        public StoreController (StoreProcessor storeProcessor) {
            _processor = storeProcessor;
        }

        [HttpPost("hat")]
		[AdminAuth]
        public ActionResult CreateHat([FromBody] NewHat hat) {
            _processor.CreateHat(hat);
            return Ok();
        }

        [HttpPost("color")]
        [AdminAuth]
		public ActionResult CreateColor([FromBody] NewColor color) {
            _processor.CreateColor(color);
            return Ok();
        }

        [HttpPost("avatar")]
        [AdminAuth]
		public ActionResult CreateAvatar([FromBody] NewAvatar avatar) {
            _processor.CreateAvatar(avatar);
            return Ok();
		}

		[HttpPost("hat/{itemId}/buy/{playerId}")]
		[PlayerAuth("playerId")]
		public ActionResult BuyHat(Guid playerId, string itemId) {
			if (_processor.BuyHat(playerId, itemId)) {
				var ok = Ok("Purchase successful!");
				return ok;
			}
			else {
				ObjectResult result = new ObjectResult("Not enough money. Or another error occurred.");
				result.StatusCode = StatusCodes.Status402PaymentRequired;
				return result;
			}
		}

	    [HttpPost("color/{itemId}/buy/{playerId}")]
	    [PlayerAuth("playerId")]
		public ActionResult BuyColor(Guid playerId, string itemId) {
			if (_processor.BuyColor(playerId, itemId)) {
				var ok = Ok("Purchase successful!");
				return ok;
			} else {
				ObjectResult result = new ObjectResult("Not enough money. Or another error occurred.");
				result.StatusCode = StatusCodes.Status402PaymentRequired;
				return result;
			}
		}

	    [HttpPost("avatar/{itemId}/buy/{playerId}")]
	    [PlayerAuth("playerId")]
		public ActionResult BuyAvatar(Guid playerId, string itemId) {
			if (_processor.BuyAvatar(playerId, itemId)) {
				var ok = Ok("Purchase successful!");
				return ok;
			} else {
				ObjectResult result = new ObjectResult("Not enough money. Or another error occurred.");
				result.StatusCode = StatusCodes.Status402PaymentRequired;
				return result;
			}
		}

		[HttpGet("hat")]
        public IEnumerable<Hat> GetAllHats() {
			return _processor.GetAllHats().Result;
        }

        [HttpGet("color")]
        public IEnumerable<Color> GetAllColors() {
			return _processor.GetAllColors().Result;
		}

        [HttpGet("avatar")]
        public IEnumerable<Avatar> GetAllAvatars() {
			Console.WriteLine("Inside StoreController, getting avatars...");
			var avatars = _processor.GetAllAvatars().Result;

			string str = "Avatars:";
			foreach (var avatar in avatars) {
				str += "\n" + avatar;
			}

			Console.WriteLine(str);

			return avatars;
		}
    }
}