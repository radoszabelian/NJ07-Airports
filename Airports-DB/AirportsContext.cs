using Airports_DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Airports_DB
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
        }

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Airline> Airlines { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Segment> Segments { get; set; }
    }
}
