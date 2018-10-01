using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Player {

		public Guid Id { get; set; }
		public string Password { get; set; }
		public string Username { get; set; }
		public string Avatar { get; set; }
		public string Color { get; set; }
		public string Hat { get; set; }
		public int Money { get; set; }
		public string Pizzeria { get; set; }
		public PlayerStatsSingle SingleStats { get; set; }
		public PlayerStatsMulti MultiStats { get; set; }
		public DateTime CreationTime { get; set; }
	}
}
