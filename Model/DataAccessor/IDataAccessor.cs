namespace Aiports_Model
{
    using System.Collections.Generic;

    public interface IDataAccessor
    {
        public IEnumerable<City> Cities { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        public IEnumerable<Airport> Airports { get; set; }

        public IEnumerable<Airline> Airlines { get; set; }

        public IEnumerable<Flight> Flights { get; set; }
    }
}
