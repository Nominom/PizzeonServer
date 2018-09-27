using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Inventory {
		Guid PlayerId;
		public List <Color> OwnedColors;
		public List <Hat> OwnedHats;
		public List <Avatar> OwnedAvatars;
	}
}
