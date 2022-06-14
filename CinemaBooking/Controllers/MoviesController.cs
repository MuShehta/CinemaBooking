using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CinemaBooking.Controllers
{
    [Authorize(AuthenticationSchemes = "cookies" , Policy ="admin")]

    public class MoviesController : Controller
    {
        private readonly appDbContext _context = new appDbContext();

        

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.movies
                .FirstOrDefaultAsync(m => m.id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,cost,available_places,is_available")] Movie movie)
        {
            if (ModelState.IsValid)
            {

                IFormFile file = Request.Form.Files[0];

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\");
                FileInfo fileInfo = new FileInfo(file.Name);
                string fileName = DateTime.Now.Ticks +file.FileName;

                file.CopyTo(new FileStream(path + fileName , FileMode.Create));

                movie.image = fileName;
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,cost,available_places,is_available")] Movie movie)
        {
            if (id != movie.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IFormFile file;
                    try
                    {
                        file = Request.Form.Files[0];
                    }
                    catch(Exception e)
                    {
                        file = null;
                    }
                    
                    if (file != null)
                    {
                        //get path of root folder
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\");
                        //make unique name for image using time of upload image
                        string fileName = DateTime.Now.Ticks + file.FileName;
                        file.CopyTo(new FileStream(path + fileName, FileMode.Create));

                        //get old image path + name
                        string old_image_path = path + "\\" + _context.movies.Find(id).image;
                        _context.ChangeTracker.Clear();

                        movie.image = fileName;
                        System.IO.File.Delete(old_image_path);
                    }
                    else
                    {
                        movie.image = _context.movies.Find(id).image;
                        _context.ChangeTracker.Clear();
                    }
                    
                    _context.Update(movie);

                    await _context.SaveChangesAsync();

                        
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.movies
                .FirstOrDefaultAsync(m => m.id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\");

            var movie = await _context.movies.FindAsync(id);
            string old_image_path = path + "\\" + movie.image;
            System.IO.File.Delete(old_image_path);

            _context.movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.movies.Any(e => e.id == id);
        }
    }
}
