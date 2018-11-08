using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Pizzeon_server.Models;

namespace PizzeonAzureFunctions.Functions
{
    public static class GetPlayerInfo
    {
        [FunctionName("GetPlayerInfo")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "players/{playerId}")]HttpRequestMessage req, TraceWriter log, string playerId)
        {
			if (!Guid.TryParse(playerId, out Guid id)) {
				return req.CreateResponse(HttpStatusCode.BadRequest, "Given Guid is not valid");
			}

	        if (!SecurityManager.CheckSecurityTokenValid(req, log, id)) {
		        return req.CreateResponse(HttpStatusCode.Unauthorized, "Security token is not valid");
	        }

			try {
				var pTask = MongoDbRepository.GetPlayer(id);
				var iTask = MongoDbRepository.GetInventory(id);

				await Task.WhenAll(pTask, iTask);
				var player = pTask.Result;
				var inventory = iTask.Result;

				PlayerInfo info = new PlayerInfo {
					Id = player.Id,
					Username = player.Username,
					Avatar = player.Avatar,
					Hat = player.Hat,
					Color = player.Color,
					Money = player.Money,
					Pizzeria = player.Pizzeria,
					Inventory = inventory
				};
				return req.CreateResponse(HttpStatusCode.OK, info);
	        } catch (Exception ex) {
				log.Error(ex+ "\nPlayer ID: " + id);
		        return req.CreateResponse(HttpStatusCode.NotFound, "No info found with playerId");
			}

		}
	}
}
