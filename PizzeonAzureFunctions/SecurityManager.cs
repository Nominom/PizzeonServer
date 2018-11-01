using Microsoft.Azure.WebJobs.Host;
using Pizzeon_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PizzeonAzureFunctions
{
	public static class SecurityManager
	{
		public static async Task AddSecurityToken(PlayerAuthorizationToken token) {

			await MongoDbRepository.CreatePlayerToken(token);
		}

		public static async Task<bool> CheckSecurityTokenValid(HttpRequestMessage req, TraceWriter log, Guid playerId) {
			try {
				if (req.Headers.TryGetValues("player_auth_key", out IEnumerable<string> values)) {
					if (!values.ToList().Contains((await MongoDbRepository.GetPlayerToken(playerId)).ApiKey)) {
						return false;
					} else {
						return true;
					}
				} else {
					return false;
				}
			} catch (Exception e) {
				return false;
			}
		}
	}
}
