using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(int id)
        {
            

            return View();
        }
    }
}
