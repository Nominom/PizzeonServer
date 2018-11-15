using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class PlayerInfo {
		public Guid Id;
		public string Username;
		public int Avatar { get; set; }
		public int Color { get; set; }
		public int Hat { get; set; }
		public int Money { get; set; }
		public string Pizzeria { get; set; }
		public Inventory Inventory { get; set; }
	}
}
