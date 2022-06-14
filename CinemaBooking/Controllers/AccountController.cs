using CinemaBooking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaBooking.Controllers
{
    public class AccountController : Controller
    {
        appDbContext _context = new appDbContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.User = "";
            return View();
        }
        [HttpPost]
        public IActionResult Register(User _user)
        {
            

            if (_user.email == null || _user.password == null || _user.password.Length < 6 || _user.user_name == null)
                return NotFound();

            var count = _context.users.Count(u => u.user_name == _user.user_name);

            if (count == 1)
            {
                ViewBag.User = "User already exist";
                return View();
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: _user.password,
            salt: new byte[128 / 8],
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            _user.password = hashed;

            _context.users.Add(_user);
            _context.SaveChanges();
            ViewBag.User = "Done";

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User _user)
        {
            var user = _context.users.FirstOrDefault(u => u.user_name == _user.user_name);
            if (user == null)
                return NotFound();

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: _user.password,
            salt: new byte[128 / 8],
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            if (hashed != user.password)
                return NotFound();

            //User user = _context.users.FirstOrDefault(u => u.user_name == _user.user_name);
            List<Claim> claims = new List<Claim>
            {
                new Claim("user" , _user.user_name),
                new Claim("id" , user.id.ToString()),
                new Claim(ClaimTypes.Role , user.isadmin == true ? "admin" : "user")
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "kkk");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(principal);

            return RedirectToAction("Index" , "Home");
        }



    }
}
