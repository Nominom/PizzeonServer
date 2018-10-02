using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Pizzeon_server {
	public class AdminAuthAttribute : ActionFilterAttribute {
		public const string AuthKey = "KissaKissa";

		public override void OnActionExecuting(ActionExecutingContext context) {
			if (context.HttpContext.Request.Headers.TryGetValue("auth_key", out StringValues values)) {
				if (!values.Contains(AuthKey)) {
					context.Result = new UnauthorizedResult();
				}
			}
			else {
				context.Result = new UnauthorizedResult();
			}
		}
	}
}
