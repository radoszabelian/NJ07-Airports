namespace NJ07_Airports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NJ07_Airports.Model;

    public interface ICacheAndDataHandler
    {
        public List<City> Cities { get; set; }

        public List<Country> Countries { get; set; }

        public List<Airport> Airports { get; set; }

        public List<Airline> Airlines { get; set; }

        public List<Flight> Flights { get; set; }
    }
}
