using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Player {

		public Guid Id { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
		public Guid Avatar { get; set; }
		public Guid Color { get; set; }
		public Guid Hat { get; set; }
		public int Coin { get; set; }
		public string Pizzeria { get; set; }
		public PlayerStatsSingle SingleStats { get; set; }
		public PlayerStatsMulti MultiStats { get; set; }
		public DateTime CreationTime { get; set; }
	}
}
