﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public class User
    {
        public bool Loged { get; set; }
        public Chart UserChart { get; set; } = new Chart();

        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; } = new Address();
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool LogedByFacebook { get; set; }

    }
}
