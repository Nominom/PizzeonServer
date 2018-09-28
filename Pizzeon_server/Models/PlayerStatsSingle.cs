using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class PlayerStatsSingle {

                public int PlayedGames { get; set; }
                public float PinpointAccuracy { get; set; }
                public float Distance { get; set; }
                public int AllPoints { get; set; }
                public int BestPoints { get; set; }
                public int Hits { get; set; }
                public int Dropped { get; set; }
                public float OverallGameTime { get; set; }
                public float BestGameTime { get; set; }
	}
}
