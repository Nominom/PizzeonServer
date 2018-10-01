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

		[HttpPost("hat/buy")]
		public ActionResult BuyHat([FromBody] ItemTransaction transaction) {
			if (_processor.BuyHat(transaction.playerId, transaction.itemId)) {
				var ok = Ok("Purchase successful!");
				return ok;
			}
			else {
				ObjectResult result = new ObjectResult("Not enough money. Or another error occurred.");
				result.StatusCode = StatusCodes.Status402PaymentRequired;
				return result;
			}
		}

		[HttpPost("color/buy")]
		public ActionResult BuyColor([FromBody] ItemTransaction transaction) {
			if (_processor.BuyColor(transaction.playerId, transaction.itemId)) {
				var ok = Ok("Purchase successful!");
				return ok;
			} else {
				ObjectResult result = new ObjectResult("Not enough money. Or another error occurred.");
				result.StatusCode = StatusCodes.Status402PaymentRequired;
				return result;
			}
		}

		[HttpPost("avatar/buy")]
		public ActionResult BuyAvatar([FromBody] ItemTransaction transaction) {
			if (_processor.BuyAvatar(transaction.playerId, transaction.itemId)) {
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
			return _processor.GetAllAvatars().Result;
		}
    }
}