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
    public static class GetStats
    {
        [FunctionName("GetStats")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "players/{playerId}/stats/{type}")]HttpRequestMessage req, TraceWriter log, string playerId, string type) {

	        if (!Guid.TryParse(playerId, out Guid id)) {
		        return req.CreateResponse(HttpStatusCode.BadRequest, "Given Guid is not valid");
			}

	        if (!SecurityManager.CheckSecurityTokenValid(req, log, id)) {
		        return req.CreateResponse(HttpStatusCode.Unauthorized, "Security token is not valid");
	        }

			if (type == "single") {
		        var stats = await MongoDbRepository.GetSingleStats(id);
		        if (stats == null) {
			        return req.CreateResponse(HttpStatusCode.NotFound, "No stats found with playerId");
				} else {
			        return req.CreateResponse(HttpStatusCode.OK, stats);
		        }
			} else if (type == "multi") {
		        var stats = await MongoDbRepository.GetMultiStats(id);
		        if (stats == null) {
			        return req.CreateResponse(HttpStatusCode.NotFound, "No stats found with playerId");
				} else {
			        return req.CreateResponse(HttpStatusCode.OK, stats);
				}
			} else {
		        return req.CreateResponse(HttpStatusCode.NotFound);
	        }

        }
    }
}
