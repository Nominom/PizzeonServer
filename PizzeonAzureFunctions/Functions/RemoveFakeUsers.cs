using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace PizzeonAzureFunctions.Functions
{
    public static class RemoveFakeUsers
    {
        [FunctionName("RemoveFakeUsers")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Admin, "get", Route = "removefakes")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Removing all fake users");
	        await MongoDbRepository.RemoveFakeUsers();
            return req.CreateResponse(HttpStatusCode.OK, "Hello");
        }
    }
}
