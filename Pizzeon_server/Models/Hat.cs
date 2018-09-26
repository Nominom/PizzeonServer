using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Hat : PurchaseableItem {
		public Guid Id { get; set;}
		public Guid Name { get; set;}
		public Guid Price { get; set;}
		public Guid Description { get; set;}
	}
}

