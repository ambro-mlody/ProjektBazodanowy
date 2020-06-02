using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GUI.Models
{
    public class Chart
    {
        public ObservableCollection<PizzaInChart> Pizzas { get; set; } = new ObservableCollection<PizzaInChart>();
        public double Price { get; set; } = 0;
    }
}
