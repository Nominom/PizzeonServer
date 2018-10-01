using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Pizzeon_server {
	public class AdminAuthAttribute : ActionFilterAttribute {
		public const string AuthKey = "KissaKissa";
		public override void OnActionExecuting(ActionExecutingContext context) {

			if (context.HttpContext.Request.Headers.TryGetValue("AuthKey", out StringValues values)) {
				values.Contains(AuthKey);
			}
		}
	}
}
