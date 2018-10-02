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

				if (id != null) {
					var playerAuthKey = PlayerAuthKeyStorage.GetToken(id.Value);
					if (playerAuthKey != null) {
						if (filterContext.HttpContext.Request.Headers.TryGetValue("player_auth_key", out StringValues values)) {
							if (!values.Contains(playerAuthKey.ApiKey)) {
								filterContext.Result = new UnauthorizedResult();
							}
						} else {
							filterContext.Result = new UnauthorizedResult();
						}
					}
					else {
						filterContext.Result = new UnauthorizedResult();
					}
				}
				else {
					filterContext.Result = new BadRequestResult();
				}
				
			}
			else {
				throw new InvalidFilterCriteriaException("Player auth filter used in a method with no playerId");
			}
		}

	}
}
