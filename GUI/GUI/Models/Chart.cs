using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GUI.Models
{
    /// <summary>
    /// Koszyk użytkownika.
    /// </summary>
    public class Chart
    {
        /// <summary>
        /// Pizze dodane do koszyka przez użytkownika.
        /// </summary>
        public ObservableCollection<PizzaInChart> Pizzas { get; set; } = new ObservableCollection<PizzaInChart>();

        /// <summary>
        /// Cena wszystkich przedmiotów w koszyku.
        /// </summary>
        public double Price { get; set; } = 0;
    }
}
