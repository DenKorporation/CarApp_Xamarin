using System;
using System.Collections.Generic;

namespace CarApp.Models
{
    public class UserData
    {
        public string Uid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CarCountry { get; set; }
        public string CarBody { get; set; }
        public string CarDrive { get; set; }
        public string Transmission { get; set; }
        public List<String> FavCars { get; set; }
    }
}