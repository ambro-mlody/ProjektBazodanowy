using System;
using System.Collections.Generic;
using System.Text;

namespace DItalia.Models
{
    /// <summary>
    /// Klasa zawierająca wszystkie dane użytkownika.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Czy użytkownik jest zalogowany w systemie.
        /// </summary>
        public bool Loged { get; set; } = false;

        /// <summary>
        /// Koszyk użytkownika.
        /// </summary>
        public Chart UserChart { get; set; } = new Chart();

        /// <summary>
        /// Identyfikator dla bazy.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Adres Email.
        /// </summary>
        public string EmailAddress { get; set; } = "";

        /// <summary>
        /// Hasło (hash).
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// Adres użytkownika (lub zamówienia).
        /// </summary>
        public Address Address { get; set; } = new Address();

        /// <summary>
        /// Numer telefonu.
        /// </summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// Imie.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Nazwisko.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// Czy użytkownik utworzył konto za pomocą autoryzacji facebooka, czy poprzez rejestrację w systemie.
        /// </summary>
        public bool LogedByFacebook { get; set; } = false;
    }
}
