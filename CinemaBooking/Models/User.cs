using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Models
{
    public class User
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool isadmin { get; set; }
        public List<Enrolled> enrolleds { get; set; }
    }
}
