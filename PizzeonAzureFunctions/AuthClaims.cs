using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace PizzeonAzureFunctions {
	public static class AuthClaims {
		private static string Secret {
			get {
				return System.Environment.GetEnvironmentVariable("ClaimsSecret");
			}
		}

		private const string claimName = "userid";

		public static string GenerateToken (Guid playerId) {
			byte[] key = Convert.FromBase64String(Secret);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(new[] {
					new Claim(claimName, playerId.ToString())}),
				Expires = DateTime.UtcNow.AddHours(24),
				SigningCredentials = new SigningCredentials(securityKey,
					SecurityAlgorithms.HmacSha256Signature)
			};

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
			return handler.WriteToken(token);
		}

		public static ClaimsPrincipal GetPrincipal (string token) {
			try {
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
				if (jwtToken == null)
					return null;
				byte[] key = Convert.FromBase64String(Secret);
				TokenValidationParameters parameters = new TokenValidationParameters() {
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
				SecurityToken securityToken;
				ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
					parameters, out securityToken);
				return principal;
			} catch (Exception e) {
				return null;
			}
		}

		public static string GetValidUserIdFromToken (string token) {
			string playerId = null;
			ClaimsPrincipal principal = GetPrincipal(token);
			if (principal == null)
				return null;
			ClaimsIdentity identity = null;
			try {
				identity = (ClaimsIdentity)principal.Identity;
			} catch (NullReferenceException) {
				return null;
			}
			Claim idClaim = identity.FindFirst(claimName);
			playerId = idClaim.Value;
			return playerId;
		}

	}//class end
}
