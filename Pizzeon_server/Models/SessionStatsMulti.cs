using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models
{
    public class SessionStatsMulti
    {
        public float PinpointAccuracy { get; set; }
        public float Distance { get; set; }
        public int Points { get; set; }
	    public int Crashes { get; set; }
		public bool Win { get; set; }
        public int Hits { get; set; }
        public int Dropped { get; set; }
        public float GameTime { get; set; }
    }
}
