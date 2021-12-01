using Aiports_Model;
using System;
using System.Collections.Generic;

namespace Airports_DB.DataAccessor
{
    public class DataAccessor : IDataAccessor
    {
        public IEnumerable<City> Cities { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<Country> Countries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<Airport> Airports { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<Airline> Airlines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<Flight> Flights { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
