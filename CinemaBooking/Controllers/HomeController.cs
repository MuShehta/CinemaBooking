using CinemaBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Controllers
{
    [Authorize(AuthenticationSchemes = "cookies")]
    public class HomeController : Controller
    {
        
        appDbContext _context = new appDbContext();
        public IActionResult Index()
        {
            return View(_context.movies.Where(e => e.is_available == true && e.available_places > 0).ToList());
        }
    }
}
