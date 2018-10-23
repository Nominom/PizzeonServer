using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Pizzeon_server {
	public class Program {
		public static void Main (string[] args) {
			var config = new ConfigurationBuilder()
				.AddJsonFile("secrets.json", false, false)
				.Build();

			CreateWebHostBuilder(args).UseConfiguration(config).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
			WebHost.CreateDefaultBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>();
	}
}
