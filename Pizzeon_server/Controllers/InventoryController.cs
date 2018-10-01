using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pizzeon_server.Models;
using Pizzeon_server.Processors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pizzeon_server.Controllers {
	[Route("api/inventory")]
	public class InventoryController : Controller {

		private InventoryProcessor _processor;

		public InventoryController (InventoryProcessor inventoryProcessor) {
			_processor = inventoryProcessor;
		}

		[HttpPut("hat")]
		public ActionResult EquipHat([FromBody] ItemTransaction transaction) {
			if (_processor.EquipHat(transaction.playerId, transaction.itemId)) {
				return Ok("Item equipped");
			}
			else {
				return UnprocessableEntity("Player not found or player doesn't own item");
			}
		}

		[HttpPut("avatar")]
		public ActionResult EquipAvatar ([FromBody] ItemTransaction transaction) {
			if (_processor.EquipAvatar(transaction.playerId, transaction.itemId)) {
				return Ok("Item equipped");
			} else {
				return UnprocessableEntity("Player not found or player doesn't own item");
			}
		}

		[HttpPut("color")]
		public ActionResult EquipColor ([FromBody] ItemTransaction transaction) {
			if (_processor.EquipColor(transaction.playerId, transaction.itemId)) {
				return Ok("Item equipped");
			} else {
				return UnprocessableEntity("Player not found or player doesn't own item");
			}
		}
	}
}
