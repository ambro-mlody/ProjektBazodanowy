using System;
using System.Collections.Generic;
using System.Text;

namespace DItalia.Models
{

	/// <summary>
	/// Klasa odpowiedzialna za hashowanie haseł.
	/// </summary>
    public static class Hashing
    {
		/// <summary>
		/// Funkcja hashująca hasło.
		/// </summary>
		/// <param name="password">Hasło w formie jawnej do shashowania.</param>
		/// <returns>Uzyskany hash.</returns>
		public static string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
		}

		/// <summary>
		/// Funkcja sprawdzająca poprawność podanego hasła wobec zapisanego hashu.
		/// </summary>
		/// <param name="password">Hasło w formie jawnej.</param>
		/// <param name="correctHash">Poprawny hash.</param>
		/// <returns>Prawda, jeżeli podane hasło odpowiada hasłu zapisanemu w hashu.</returns>
		public static bool ValidatePassword(string password, string correctHash)
		{
			return BCrypt.Net.BCrypt.EnhancedVerify(password, correctHash);
		}
	}
}
