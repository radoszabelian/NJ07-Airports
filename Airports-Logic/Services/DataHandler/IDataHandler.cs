namespace Airports_Logic.Services
{
    using System.Collections.Generic;
    using Airports_Logic.Model;

    public interface IDataHandler
    {
        public List<City> Cities { get; set; }

        public List<Country> Countries { get; set; }

        public List<Airport> Airports { get; set; }

        public List<Airline> Airlines { get; set; }

        public List<Flight> Flights { get; set; }
    }
}
