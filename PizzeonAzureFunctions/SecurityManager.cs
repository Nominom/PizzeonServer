using Microsoft.Azure.WebJobs.Host;
using Pizzeon_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PizzeonAzureFunctions
{
	public static class SecurityManager
	{
		/*
		public static async Task AddSecurityToken(PlayerAuthorizationToken token) {
			Cache.Set(TokenCacheKey(token), token.ApiKey, DateTimeOffset.Now.AddHours(1));
			await MongoDbRepository.CreatePlayerToken(token);
		}

		public static async Task<bool> CheckSecurityTokenValid(HttpRequestMessage req, TraceWriter log, Guid playerId) {
			try {
				if (req.Headers.TryGetValues("player_auth_key", out IEnumerable<string> values)) {
					string apiKey = Cache[TokenCacheKey(playerId)] as string;
					if (string.IsNullOrEmpty(apiKey)) { // if no cache item was found, get from DB
						apiKey = (await MongoDbRepository.GetPlayerToken(playerId)).ApiKey;
						Cache.Set(TokenCacheKey(playerId), apiKey, DateTimeOffset.Now.AddHours(1));
						log.Info("Used database for API key");
					} else {
						log.Info("Found cached API key");
					}

					var headers = values.ToList();
					if (!headers.Contains(apiKey)) {
						//cache might have an old value, get new one from database
						apiKey = (await MongoDbRepository.GetPlayerToken(playerId)).ApiKey;
						Cache.Set(TokenCacheKey(playerId), apiKey, DateTimeOffset.Now.AddHours(1));
						log.Info("API key was old in cache, refreshing...");
						return headers.Contains(apiKey);
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

		private static string TokenCacheKey(PlayerAuthorizationToken token) {
			return TokenCacheKey(token.PlayerId);
		}

		private static string TokenCacheKey (Guid playerId) {
			return "token_" + playerId.ToString();
		}*/


		public static PlayerAuthorizationToken CreateAuthorizationToken (Guid playerId) {
			PlayerAuthorizationToken token = new PlayerAuthorizationToken();
			token.PlayerId = playerId;
			token.ApiKey = AuthClaims.GenerateToken(playerId);
			return token;
		}

		public static bool CheckSecurityTokenValid(HttpRequestMessage req, TraceWriter log, Guid playerId) {
			if (req.Headers.TryGetValues("player_auth_key", out IEnumerable<string> values)) {
				//Get token from header
				var token = values.FirstOrDefault();
				if (string.IsNullOrEmpty(token))
					return false;

				//Get playerId in token from token string
				var tokenId = AuthClaims.GetValidUserIdFromToken(token);
				if (string.IsNullOrEmpty(tokenId))
					return false;
				
				//check if token playerId matches with provided playerId
				if (tokenId.Equals(playerId.ToString())) {
					log.Info("Valid token for playerid: "+playerId);
					return true;
				}
			}
			//return false if tokens didn't match or header contained no auth key
			return false;
		}
	}
}
