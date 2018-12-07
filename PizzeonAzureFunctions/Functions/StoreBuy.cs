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
    public static class StoreBuy
    {
        [FunctionName("StoreBuy")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "store/{type}/{itemId:int}/buy/{playerId}")]HttpRequestMessage req, TraceWriter log, string type, int itemId, string playerId)
        {
			if (!Guid.TryParse(playerId, out Guid pId)) {
		        return req.CreateResponse(HttpStatusCode.BadRequest, "Given Guid is not valid");
			}

			if (!SecurityManager.CheckSecurityTokenValid(req, log, pId)) {
				return req.CreateResponse(HttpStatusCode.Unauthorized, "Security token is not valid");
			}

			try {
		        int price = 0;
		        switch (type) {
			        case ("hat"):
				        var hat = await MongoDbRepository.GetHat(itemId, log);
				        price = hat.Price;
				        break;
			        case ("color"):
						var color = await MongoDbRepository.GetColor(itemId, log);
				        price = color.Price;
						break;
			        case ("avatar"):
						var avatar = await MongoDbRepository.GetAvatar(itemId, log);
				        price = avatar.Price;
						break;
			        default:
				        return req.CreateResponse(HttpStatusCode.NotFound);
		        }


				//Check if player has item already
				switch (type) {
					case ("hat"):
						if (await MongoDbRepository.InventoryHasHat(pId, itemId)) {
							return req.CreateResponse(HttpStatusCode.Forbidden, "Player already owns item!");
						}
						break;
					case ("color"):
						if (await MongoDbRepository.InventoryHasColor(pId, itemId)) {
							return req.CreateResponse(HttpStatusCode.Forbidden, "Player already owns item!");
						}
						break;
					case ("avatar"):
						if (await MongoDbRepository.InventoryHasAvatar(pId, itemId)) {
							return req.CreateResponse(HttpStatusCode.Forbidden, "Player already owns item!");
						}
						break;
					default:
						return req.CreateResponse(HttpStatusCode.NotFound);
				}

				Player player = await MongoDbRepository.GetPlayer(pId);
		        if (player.Money >= price) {
			        Task dTask = MongoDbRepository.DeductCoinFromPlayer(pId, price);
			        Task aTask;
			        switch (type) {
				        case ("hat"):
					        aTask = MongoDbRepository.AddHatToInventory(pId, itemId);
					        break;
						case ("color"):
							aTask = MongoDbRepository.AddColorToInventory(pId, itemId);
					        break;
				        case ("avatar"):
					        aTask = MongoDbRepository.AddAvatarToInventory(pId, itemId);
					        break;
						default:
							aTask = Task.CompletedTask;
							break;
					}

			        await Task.WhenAll(dTask, aTask);

			        return req.CreateResponse(HttpStatusCode.OK);
				}
		        else {
			        return req.CreateResponse(HttpStatusCode.PaymentRequired);
				}

			} catch (Exception ex) {
		        log.Warning(ex.Message);
	        }

	        return req.CreateResponse(HttpStatusCode.BadRequest);
		}
    }
}
