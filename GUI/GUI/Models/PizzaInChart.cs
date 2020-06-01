using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public class PizzaInChart
    {
        public int Amount { get; set; }
        public PizzaItem Pizza { get; set; }
        public double TotalCost { get; set; }
        public SizeStruct Size { get; set; }

    }
}
