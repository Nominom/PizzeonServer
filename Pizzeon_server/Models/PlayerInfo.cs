using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class PlayerInfo {
		public Guid Id;
		public string Username;
		public Guid Avatar { get; set; }
		public Guid Color { get; set; }
		public Guid Hat { get; set; }
		public int Money { get; set; }
		public string Pizzeria { get; set; }
	}
}
