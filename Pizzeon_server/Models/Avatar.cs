using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeon_server.Models {
	public class Avatar : PurchaseableItem {
		public Guid Id { get; set;}
		public string Name { get; set;}
		public int Price { get; set;}
	}
}
