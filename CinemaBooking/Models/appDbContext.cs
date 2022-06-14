using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Models
{
    public class appDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer("Data Source=DESKTOP-ELJAV78\\SQLEXPRESS;Initial Catalog=MovieBooking;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrolled>().HasKey("userid", "movieid");
            modelBuilder.Entity<Enrolled>().HasOne("movie").WithMany("enrolleds");
            modelBuilder.Entity<Enrolled>().HasOne("user").WithMany("enrolleds");
            
        }

        public DbSet<User> users { get; set; }
        public DbSet<Movie> movies { get; set; }
        public DbSet<Enrolled> enrolleds { get; set; }



        

    }
}
