using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Inventory {
		public Guid PlayerId { get; set; }
		public List <Guid> OwnedColors;
		public List <Guid> OwnedHats;
		public List <Guid> OwnedAvatars;
	}
}
