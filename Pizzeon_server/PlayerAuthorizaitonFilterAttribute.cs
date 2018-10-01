using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Pizzeon_server {
	public class PlayerAuthAttribute : ActionFilterAttribute {

		public string PlayerIdParamName { get; set; }

		public PlayerAuthAttribute (string paramName) {
			PlayerIdParamName = paramName;
		}

		public override void OnActionExecuting (ActionExecutingContext filterContext) {
			if (filterContext.ActionArguments.ContainsKey(PlayerIdParamName)) {
				var id = filterContext.ActionArguments[PlayerIdParamName] as Guid?;

				string PlayerAuthKey = "testauth"; // get from database

				if (filterContext.HttpContext.Request.Headers.TryGetValue("player_auth_key", out StringValues values)) {
					if (!values.Contains(PlayerAuthKey)) {
						filterContext.Result = new UnauthorizedResult();
					}
				} else {
					filterContext.Result = new UnauthorizedResult();
				}
			}
			else {
				throw new InvalidFilterCriteriaException("Player auth filter used in a method with no playerId");
			}
		}

	}
}
