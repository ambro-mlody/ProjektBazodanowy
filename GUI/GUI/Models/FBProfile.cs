using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    /// <summary>
    /// Profil użytkownika pobierany z API z faccebooka.
    /// </summary>
    public class FBProfile
    {
        /// <summary>
        /// Adres email.
        /// </summary>
        public string email { get; set; } = "";

        /// <summary>
        /// Imie.
        /// </summary>
        public string first_name { get; set; } = "";

        /// <summary>
        /// Nazwisko.
        /// </summary>
        public string last_name { get; set; } = "";

    }
}
