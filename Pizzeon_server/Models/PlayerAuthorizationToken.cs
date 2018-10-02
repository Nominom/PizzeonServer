using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Pizzeon_server.Models {
	public class PlayerAuthorizationToken {
		[BsonId] 
		public Guid PlayerId { get; set; }
		public string ApiKey { get; set; }

		public static PlayerAuthorizationToken Create(Guid playerId) {
			var token = new PlayerAuthorizationToken {
				ApiKey = Guid.NewGuid().ToString(),
				PlayerId = playerId
			};
			return token;
		}
	}
}
