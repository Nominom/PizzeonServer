using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Pizzeon_server.Models {
	public class Inventory {
		[BsonId]
		public Guid PlayerId { get; set; }
		public List <int> OwnedColors;
		public List <int> OwnedHats;
		public List <int> OwnedAvatars;
	}
}
