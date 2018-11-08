/*using System;
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
    public static class GetPlayerInventory
    {
        [FunctionName("GetPlayerInventory")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "inventory/{playerId}")]HttpRequestMessage req, TraceWriter log, string playerId)
        {
			if (!Guid.TryParse(playerId, out Guid id)) {
				return req.CreateResponse(HttpStatusCode.BadRequest, "Given Guid is not valid");
			}

			if (!SecurityManager.CheckSecurityTokenValid(req, log, id)) {
				return req.CreateResponse(HttpStatusCode.Unauthorized, "Security token is not valid");
			}

			try {
		        var inventory = await MongoDbRepository.GetInventory(id);
				
				return req.CreateResponse(HttpStatusCode.OK, inventory);
	        } catch (Exception) {
		        return req.CreateResponse(HttpStatusCode.NotFound, "No inventory found with playerId");
			}

		}
	}
}
*/