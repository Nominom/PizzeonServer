using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pizzeon_server.Models {
	public class PlayerStatsMulti {

		public int PlayedGames { get; set; }
		[BsonRepresentation(BsonType.Double, AllowTruncation = true)]
		public float PinpointAccuracy { get; set; }
		[BsonRepresentation(BsonType.Double, AllowTruncation = true)]
		public float Distance { get; set; }
		public int Crashes { get; set; }
		public int AllPoints { get; set; }
		public int BestPoints { get; set; }
		public int Win { get; set; }
		public int Loss { get; set; }
		public int Hits { get; set; }
		public int Dropped { get; set; }
		[BsonRepresentation(BsonType.Double, AllowTruncation = true)]
		public float OverallGameTime { get; set; }
	}
}
