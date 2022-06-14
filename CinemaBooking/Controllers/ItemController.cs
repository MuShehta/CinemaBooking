using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace CinemaBooking.Controllers
{
    public class ItemController : Controller
    {
        appDbContext _context = new appDbContext();
        [HttpGet]
        public IActionResult Index(int? id)
        {
            if (id == null)
                return NotFound();

            var movie = _context.movies.Find(id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        public IActionResult Index()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            string? user_email = Request.Form["email"];
            Movie movie = _context.movies.Find(id);

            if (user_email == null && movie.available_places > 0)
                return NotFound();

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("email@gmail.com", "password"),
                EnableSsl = true,
            };

            smtpClient.Send("email@gmail.com", user_email, "Cima Booking", "you are already booked " + movie.name);


            return View(movie);
        }
    }
}
