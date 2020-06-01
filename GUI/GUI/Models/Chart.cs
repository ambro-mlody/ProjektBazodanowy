using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public class Chart
    {
        public List<PizzaInChart> Pizzas { get; set; } = new List<PizzaInChart>();
        public double Price { get; set; } = 0;
    }
}
