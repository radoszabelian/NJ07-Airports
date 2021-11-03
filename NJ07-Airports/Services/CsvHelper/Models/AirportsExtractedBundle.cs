namespace NJ07_Airports.Services.CsvHelper.Models
{
    using System.Collections.Generic;
    using NJ07_Airports.Model;

    public class AirportsExtractedBundle
    {
        public List<Airport> Airports { get; set; }

        public List<City> Cities { get; set; }

        public List<Country> Countries { get; set; }
    }
}
