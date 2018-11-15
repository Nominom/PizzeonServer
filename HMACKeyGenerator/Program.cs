using System;
using System.Security.Cryptography;

namespace HMACKeyGenerator {
	class Program {
		static void Main (string[] args) {

			HMACSHA256 hmac = new HMACSHA256();
			string key = Convert.ToBase64String(hmac.Key);

			Console.WriteLine(key);
			Console.ReadLine();
		}
	}
}
