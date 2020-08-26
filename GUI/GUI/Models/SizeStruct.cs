using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    /// <summary>
    /// Klasa opisująca różne rozmiary pizzz do wyboru.
    /// </summary>
    public class SizeStruct
    {
        /// <summary>
        /// Średnica pizzy.
        /// </summary>
        public int Size;

        /// <summary>
        /// Nazwa rozmiaru np. Duża.
        /// </summary>
        public string Name;

        /// <summary>
        /// Nadpisana funkcja ToString dla strony wybotu rozmiaru.
        /// </summary>
        /// <returns>{Nazwa Rozmiar cm}</returns>
        public override string ToString()
        {
            return $"{Name} {Size} cm";
        }
    }
}
