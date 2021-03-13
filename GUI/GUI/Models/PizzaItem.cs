using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DItalia.Models
{
    /// <summary>
    /// Dane pizzy.
    /// </summary>
    public class PizzaItem
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa pizzy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cena.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Zdjęcie poglądowe.
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// Opis.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Czas przygotowania.
        /// </summary>
        public int PreperationTime { get; set; }

        /// <summary>
        /// Lista składników (na razie string, ale mogą zostać zamienione na klasę w przyszłości).
        /// </summary>
        public List<string> Ingredients { get; set; }

    }
}
