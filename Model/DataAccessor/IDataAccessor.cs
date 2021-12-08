namespace Aiports_Model
{
    using System.Collections.Generic;

    public interface IDataAccessor
    {
        public IEnumerable<City> Cities { get; }

        public IEnumerable<Country> Countries { get; }

        public IEnumerable<Airport> Airports { get; }

        public IEnumerable<Airline> Airlines { get; }

        public IEnumerable<Flight> Flights { get; }

        public IEnumerable<Segment> Segments { get; }
    }
}
