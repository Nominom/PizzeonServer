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
    public static class AddStats
    {
        [FunctionName("AddStats")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "players/{playerId}/stats/{type}")]HttpRequestMessage req, TraceWriter log, string playerId, string type)
        {
			
			if (!Guid.TryParse(playerId, out Guid id)) {
		        return req.CreateResponse(HttpStatusCode.BadRequest, "Given Guid is not valid");
	        }

			if (!SecurityManager.CheckSecurityTokenValid(req, log, id)) {
				return req.CreateResponse(HttpStatusCode.Unauthorized, "Security token is not valid");
			}

			if (type == "single") {

		        dynamic data = await req.Content.ReadAsAsync<SessionStatsSingle>();
		        if (data == null) {
			        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a valid json object in the body");
		        }

				try {
					await MongoDbRepository.AddStatsSingle(id, data);
                    int coin = (int)(data.Points * 0.05);
                    await MongoDbRepository.AddCoinToPlayer(id, coin);
                } catch (Exception ex) {
					return req.CreateResponse(HttpStatusCode.BadRequest, "An error occurred");
				}

				return req.CreateResponse(HttpStatusCode.OK);
	        
	        } else if (type == "multi") {
		        dynamic data = await req.Content.ReadAsAsync<SessionStatsMulti>();
		        if (data == null) {
			        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a valid json object in the body");
		        }

		        try {
			        await MongoDbRepository.AddStatsMulti(id, data);
                    int coin = (int)(data.Points * 0.10);
                    await MongoDbRepository.AddCoinToPlayer(id, coin);
                    log.Info("give money");                }
		        catch (Exception ex) {
			        return req.CreateResponse(HttpStatusCode.BadRequest, "An error occurred");
				}

				return req.CreateResponse(HttpStatusCode.OK);
	        
	        } else {
		        return req.CreateResponse(HttpStatusCode.NotFound);
	        }
		}
    }
}
