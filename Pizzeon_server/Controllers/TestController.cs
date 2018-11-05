using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pizzeon_server.Controllers {
	[Route("api/[controller]")]
	public class TestController : Controller {
		private const string VersionNumber = "0005";

		// GET: api/<controller>
		[HttpGet]
		public string Get () {
			return "All OK! Version: " + VersionNumber;
		}
	}
}
