using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace PizzeonAzureFunctions.Functions
{
    public static class GetAllItems
    {
        [FunctionName("GetAllItems")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "store/{type}")]HttpRequestMessage req, TraceWriter log, string type)
        {
            log.Info("Getting all things");
	        switch (type) {
				case "hat":
					var hats = await MongoDbRepository.GetAllHats(log);
					return req.CreateResponse(HttpStatusCode.OK, hats);
		        case "color":
			        var colors = await MongoDbRepository.GetAllColors(log);
			        return req.CreateResponse(HttpStatusCode.OK, colors);
		        case "avatar":
			        var avatars = await MongoDbRepository.GetAllAvatars(log);
			        return req.CreateResponse(HttpStatusCode.OK, avatars);
				default:
					return req.CreateResponse(HttpStatusCode.NotFound);
			}
        }
    }
}
