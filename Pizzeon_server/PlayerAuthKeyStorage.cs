using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server {
	public class PlayerAuthKeyStorage {

		public static ConcurrentDictionary<Guid, PlayerAuthorizationToken> Tokens = new ConcurrentDictionary<Guid, PlayerAuthorizationToken>();

		public static PlayerAuthorizationToken GetToken(Guid playerId) {
			if (Tokens.ContainsKey(playerId)) {
				return Tokens[playerId];
			}
			else {
				return null;
			}
		}

		public static void AddToken(PlayerAuthorizationToken token) {
			Tokens.AddOrUpdate(token.PlayerId, token, (key, oldValue) => new PlayerAuthorizationToken(){PlayerId = token.PlayerId, ApiKey = token.ApiKey});
		}
	}
}
