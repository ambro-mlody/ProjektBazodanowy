using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DItalia.Models
{
    /// <summary>
    /// Klasa zawierająca informację o pizzy znajdującej się w koszyku użytkownika.
    /// </summary>
    public class PizzaInChart
    {
        /// <summary>
        /// Ilość tego typu pizz w koszyku.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Dane dotyczące pizzy.
        /// </summary>
        public PizzaItem Pizza { get; set; } = new PizzaItem();

        /// <summary>
        /// Koszt jednej sztuki pizzy.
        /// </summary>
        public double CostOfOne { get; set; }

        /// <summary>
        /// Łączny koszt (koszt jednej * ilość).
        /// </summary>
        public double TotalCost { get; set; }

        /// <summary>
        /// Wielkość pizzy.
        /// </summary>
        public SizeStruct Size { get; set; } = new SizeStruct();

        /// <summary>
        /// Identyfikator dla bazy.
        /// </summary>
        public int Id { get; set; }

    }
}
