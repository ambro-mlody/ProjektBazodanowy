using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public class User
    {
        public bool Loged { get; set; }
        public Chart UserChart { get; set; } = new Chart();

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

    }
}
