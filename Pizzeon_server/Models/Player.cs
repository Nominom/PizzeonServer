using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Player {

		public Guid Id { get; set; }
		public Guid Password { get; set; }
		public Guid Username { get; set; }
		public Guid Avatar { get; set; }
		public Guid Color { get; set; }
		public Guid Hat { get; set; }
		public Guid Coin { get; set; }
		public Guid Pizzeria { get; set; }
		public Guid Stats { get; set; }
	}
}
