using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    /// <summary>
    /// Klasa przechowująca informację o adresie.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Id adresu (rekordu w tabeli).
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Miasto.
        /// </summary>
        public string City { get; set; } = "";

        /// <summary>
        /// Ulica.
        /// </summary>
        public string Street { get; set; } = "";

        /// <summary>
        /// Numer domu.
        /// </summary>
        public string HouseNumber { get; set; } = "";

        /// <summary>
        /// Numer mieszkania.
        /// </summary>
        public string LocalNumber { get; set; } = "";

        /// <summary>
        /// Kod pocztowy.
        /// </summary>
        public string PostCode { get; set; } = "";

    }
}
