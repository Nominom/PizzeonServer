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

		[HttpPut("hat/{itemId}/equip/{playerId}")]
		[PlayerAuth("playerId")]
		public ActionResult EquipHat(Guid playerId, string itemId) {
			if (_processor.EquipHat(playerId, itemId)) {
				return Ok("Item equipped");
			}
			else {
				return UnprocessableEntity("Player not found or player doesn't own item");
			}
		}

		[HttpPut("avatar/{itemId}/equip/{playerId}")]
		[PlayerAuth("playerId")]
		public ActionResult EquipAvatar (Guid playerId, string itemId) {
			if (_processor.EquipAvatar(playerId, itemId)) {
				return Ok("Item equipped");
			} else {
				return UnprocessableEntity("Player not found or player doesn't own item");
			}
		}

		[HttpPut("color/{itemId}/equip/{playerId}")]
		[PlayerAuth("playerId")]
		public ActionResult EquipColor (Guid playerId, string itemId) {
			if (_processor.EquipColor(playerId, itemId)) {
				return Ok("Item equipped");
			} else {
				return UnprocessableEntity("Player not found or player doesn't own item");
			}
		}

		[HttpGet("{playerId}")]
		[PlayerAuth("playerId")]
		public ActionResult GetInventory(Guid playerId) {
			var inventory = _processor.GetInventory(playerId).Result;

			if (inventory == null) {
				return UnprocessableEntity("Bad inventory query.");
			}
			else {
				return new JsonResult(inventory);
			}
		}
	}
}
