using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pizzeon_server.Models {
	public class PlayerStatsView {
		public string Username { get; set; }
		public string Pizzeria { get; set; }
		public int PlayedGames { get; set; }
		[BsonRepresentation(BsonType.Double, AllowTruncation = true)]
		public float Accuracy { get; set; }
		public float Distance { get; set; }
		public int AllPoints { get; set; }
		public int BestPoints { get; set; }
		public int PizzasDelivered { get; set; }
	}
}
