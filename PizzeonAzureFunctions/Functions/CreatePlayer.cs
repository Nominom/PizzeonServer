using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Pizzeon_server.Models;

namespace PizzeonAzureFunctions
{
    public static class CreatePlayer
    {
        [FunctionName("CreatePlayer")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "players")]HttpRequestMessage req, TraceWriter log)
        {
			
            // Get request body
            dynamic data = await req.Content.ReadAsAsync<NewPlayer>();
	        if (data == null) {
		        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a valid json object in the body");
	        }

			if (!await MongoDbRepository.CheckUsernameAvailable(data.Username)) {
		        return req.CreateResponse(HttpStatusCode.Conflict, "Username already taken!");
	        }

	        log.Info("Creating a new user "+ data.Username);


			Player player = new Player();
	        player.Username = data.Username;
	        player.PasswordSalt = StaticHelpers.GetRandomSalt();
	        player.Password = StaticHelpers.EncodePasswordToBase64(data.Password + player.PasswordSalt);
	        player.Id = Guid.NewGuid();
	        player.CreationTime = System.DateTime.Now;
	        player.Hat = 0;
	        player.Color = 0;
	        player.Avatar = 0;
	        player.Money = 0;
	        string pizzeriaUsername = player.Username.First().ToString().ToUpper() + player.Username.Substring(1);
	        player.Pizzeria = pizzeriaUsername + "'s Pizzeria";
	        player.SingleStats = new PlayerStatsSingle();
	        player.MultiStats = new PlayerStatsMulti();

	        await MongoDbRepository.CreatePlayer(player);
	        await InternalFunctions.CreatePlayerInventory(player.Id);

	        return req.CreateResponse(HttpStatusCode.OK, "Player created!");
        }
    }
}
