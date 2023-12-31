﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace nov27_task
{
	public static class HashPassword
	{
		const int keySize = 64;
		const int iterations = 350000;
		static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

		public static byte[] HashingPass(string password, byte[] salt)
		{
			var hash = Rfc2898DeriveBytes.Pbkdf2(
				Encoding.UTF8.GetBytes(password),
				salt,
				iterations,
				hashAlgorithm,
				keySize);

			return hash;
		}
	}
}
