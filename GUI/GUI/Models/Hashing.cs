using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public static class Hashing
    {

		public static string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
		}

		public static bool ValidatePassword(string password, string correctHash)
		{
			return BCrypt.Net.BCrypt.EnhancedVerify(password, correctHash);
		}
	}
}
