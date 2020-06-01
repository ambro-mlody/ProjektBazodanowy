using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public class SizeStruct
    {
        public int Size;
        public string Name;

        public override string ToString()
        {
            return $"{Name} {Size} cm";
        }
    }
}
