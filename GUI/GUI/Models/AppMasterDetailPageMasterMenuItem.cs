using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DItalia.Models
{
    /// <summary>
    /// Pole w bocznym menu.
    /// </summary>
    public class AppMasterDetailPageMasterMenuItem
    {
        public AppMasterDetailPageMasterMenuItem()
        {
            TargetType = typeof(AppMasterDetailPageMasterMenuItem);
        }

        /// <summary>
        /// Id pola.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tytuł strony do której prowadzi to pole.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Typ wyświetlanej karty
        /// </summary>
        public Type TargetType { get; set; }

        /// <summary>
        /// Ikona w menu.
        /// </summary>
        public string Image { get; set; }
    }
}