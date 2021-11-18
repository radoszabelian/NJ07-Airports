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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        // entities
        // todo majd ide jönnek az entityk
        // public DbSet<Airport> Airports { get; set; }
    }
}
