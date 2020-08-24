using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GUI.Models
{
    public class PizzaItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ImageSource Image { get; set; }
        public string Description { get; set; }
        public int PreperationTime { get; set; }
        public List<string> Ingredients { get; set; }

    }
}
