using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.Models
{
    public class PizzaInChart
    {
        public int Amount { get; set; }
        public PizzaItem Pizza { get; set; } = new PizzaItem();
        public double CostOfOne { get; set; }
        public double TotalCost { get; set; }
        public SizeStruct Size { get; set; } = new SizeStruct();
        public int Id { get; set; }

    }
}
