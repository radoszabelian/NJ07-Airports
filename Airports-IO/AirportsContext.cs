using Airports_IO.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airports_IO
{
    public class AirportsContext : DbContext
    {
        public AirportsContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>()
                .Property(a => a.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<Airline>()
                .Property(a => a.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<City>()
                .Property(a => a.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<Country>()
                .Property(a => a.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<Flight>()
                .Property(a => a.Id)
                .ValueGeneratedNever();
        }

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Airline> Airlines { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Segment> Segments { get; set; }
    }
}
