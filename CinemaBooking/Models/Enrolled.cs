using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Models
{
    public class Enrolled
    {
        public User user { get; set; }
        public Movie movie { get; set; }
        public DateTime date { get; set; }
    }
}
