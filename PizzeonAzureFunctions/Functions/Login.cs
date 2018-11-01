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
	public static class Login
	{
		[FunctionName("Login")]
		public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "players/login")]HttpRequestMessage req, TraceWriter log) {
			log.Info("New Login request");

			dynamic data = await req.Content.ReadAsAsync<LoginCredentials>();

			if (data == null) {
				return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a valid json object in the body");
			}

			try {
				Player player = MongoDbRepository.GetPlayerByName(data.Username).Result;

				if (player.Password == StaticHelpers.EncodePasswordToBase64(data.Password + player.PasswordSalt)) {
					var token = PlayerAuthorizationToken.Create(player.Id);
					await SecurityManager.AddSecurityToken(token);
					return req.CreateResponse(HttpStatusCode.OK, token);

				} else {
					return req.CreateResponse(HttpStatusCode.Unauthorized, "Bad password");
				}
			} catch (Exception) {
				return req.CreateResponse(HttpStatusCode.Unauthorized, "No username");
			}
		}
	}
}
