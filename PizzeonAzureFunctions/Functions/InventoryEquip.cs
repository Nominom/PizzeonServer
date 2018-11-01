using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace PizzeonAzureFunctions.Functions
{
    public static class InventoryEquip
    {
        [FunctionName("InventoryEquip")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "inventory/{type}/{itemId}/equip/{playerId}")]HttpRequestMessage req, TraceWriter log, string type, string itemId, string playerId)
        {
	        if (!Guid.TryParse(playerId, out Guid pId)) {
		        return req.CreateResponse(HttpStatusCode.BadRequest, "Given Guid is not valid");
			}

			if (!await SecurityManager.CheckSecurityTokenValid(req, log, pId)) {
				return req.CreateResponse(HttpStatusCode.Unauthorized, "Security token is not valid");
			}

			try {
		        switch (type) {
			        case ("hat"):
				        if (await MongoDbRepository.InventoryHasHat(pId, itemId)) {
					        await MongoDbRepository.EquipHat(pId, itemId);
					        return req.CreateResponse(HttpStatusCode.OK);
						}
						break;
			        case ("color"):
				        if (await MongoDbRepository.InventoryHasColor(pId, itemId)) {
					        await MongoDbRepository.EquipColor(pId, itemId);
					        return req.CreateResponse(HttpStatusCode.OK);
						}
						break;
			        case ("avatar"):
				        if (await MongoDbRepository.InventoryHasAvatar(pId, itemId)) {
					        await MongoDbRepository.EquipAvatar(pId, itemId);
					        return req.CreateResponse(HttpStatusCode.OK);
						}
						break;
			        default:
				        return req.CreateResponse(HttpStatusCode.NotFound);
		        }
	        }
	        catch (Exception ex) {
				log.Warning(ex.Message);
			}

	        return req.CreateResponse(HttpStatusCode.BadRequest);
		}
	}
}
