using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PizzeonAzureFunctions {
	public static class StaticHelpers {

		public static string EncodePasswordToBase64 (string password) {
			byte[] bytes = Encoding.Unicode.GetBytes(password);
			//Could use Bcrypt or something later for more security
			byte[] inArray = HashAlgorithm.Create("SHA256").ComputeHash(bytes);
			return Convert.ToBase64String(inArray);
		}

		public static string GetRandomSalt () {
			byte[] salt = new byte[32];
			RNGCryptoServiceProvider.Create().GetBytes(salt);
			return Convert.ToBase64String(salt);
		}
	}
}
