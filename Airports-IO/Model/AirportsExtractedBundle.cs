namespace Airports_IO.Model
{
    using System.Collections.Generic;

    public class AirportsExtractedBundle
    {
        public List<Airport> Airports { get; set; }

        public List<City> Cities { get; set; }

        public List<Country> Countries { get; set; }
    }
}
