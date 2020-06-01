using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public class PizzaItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int PreperationTime { get; set; }
        public List<string> Ingredients { get; set; }

    }
}
