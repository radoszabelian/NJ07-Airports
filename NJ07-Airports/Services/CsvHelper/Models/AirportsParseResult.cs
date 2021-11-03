namespace NJ07_Airports.Services.CsvHelper.Models
{
    public class AirportsParseResult
    {
        public string Id { get; set; }

        public string AirportName { get; set; }

        public string CityName { get; set; }

        public string CountryName { get; set; }

        public string IATA { get; set; }

        public string ICAO { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Altitude { get; set; }
    }
}
