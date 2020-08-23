using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.Models
{
    public static class DBConnection
    {
        public static string ConnectionString { get; set; } = @"Data Source=192.168.0.100\MSSQLSERVEWELL;Initial Catalog=pizzeriaDB;User ID=PizzeriaLogin;Password=Pizzeria";
    }
}
