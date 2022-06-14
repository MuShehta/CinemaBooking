using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Models
{
    public class Movie
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public int cost { get; set; }
        public int available_places { get; set; }
        public bool is_available { get; set; }
        public List<Enrolled> enrolleds { get; set; }
    }
}
