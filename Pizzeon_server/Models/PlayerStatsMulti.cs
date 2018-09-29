using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class PlayerStatsMulti {

		public int PlayedGames { get; set; }
		public float PinpointAccuracy { get; set; }
		public float Distance { get; set; }
		public int Crashes { get; set; }
		public int AllPoints { get; set; }
		public int BestPoints { get; set; }
		public int Win { get; set; }
		public int Loss { get; set; }
		public int Hits { get; set; }
		public int Dropped { get; set; }
		public float OverallGameTime { get; set; }
	}
}
