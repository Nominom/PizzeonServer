using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace PizzeonAzureFunctions.Functions
{
    public static class GetScoreboard
    {
        [FunctionName("GetScoreboard")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "scoreboard/{type}")]HttpRequestMessage req, TraceWriter log, string type)
        {

            // parse query parameter
            string numStr = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "number", true) == 0)
                .Value;

	        string pageStr = req.GetQueryNameValuePairs()
		        .FirstOrDefault(q => string.Compare(q.Key, "page", true) == 0)
		        .Value;

	        if (!int.TryParse(numStr, out int number)) {
		        number = 10;
	        }

	        if (!int.TryParse(pageStr, out int page)) {
		        page = 0;
	        }

	        if (number > 100)
		        return req.CreateResponse(HttpStatusCode.BadRequest, "number requested cannot be more than 100");

			switch (type) {
				case ("single"):
					var single = await MongoDbRepository.GetTopPlayerStatsSingle(number, page);
					return req.CreateResponse(HttpStatusCode.OK, single);
				case ("multi"):
					var multi = await MongoDbRepository.GetTopPlayerStatsMulti(number, page);
					return req.CreateResponse(HttpStatusCode.OK, multi);
				default:
					return req.CreateResponse(HttpStatusCode.NotFound);
			}
        }
    }
}
