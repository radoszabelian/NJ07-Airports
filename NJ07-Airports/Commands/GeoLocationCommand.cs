namespace Airports_CLI
{
    using System;
    using System.Text.RegularExpressions;
    using Airports_Logic.Services.GeoLocation;

    public class GeoLocationCommand : ICommand
    {
        private IGeoLocationService geoLocationService;

        public GeoLocationCommand(IGeoLocationService geoLocationService)
        {
            this.geoLocationService = geoLocationService;
        }

        public void Start()
        {
            Console.WriteLine("Please enter GPS Coordinate or IATA Code!");
            string input = Console.ReadLine();

            Regex gpsCoordinateSample = new Regex(@"^\-?\d*\.\d*\, \-?\d*.\d*, \d*$");
            Regex iataSample = new Regex(@"^\b[A-Z0-9]{2,3}\b$");

            if (gpsCoordinateSample.IsMatch(input))
            {
                var airport = this.geoLocationService.GetClosestAirportByGps(input);

                if (airport != null)
                {
                    Console.WriteLine($"The closest airport is: {airport.Name}");
                } else
                {
                    Console.WriteLine($"Could not find anything with these coordinates: {input}");
                }
            }

            if (iataSample.IsMatch(input))
            {
                var airport = this.geoLocationService.GetAirportByIATA(input);
                if (airport != null)
                {
                    Console.WriteLine($"The aiport with IATA: {input} is: {airport.Name}");
                } else
                {
                    Console.WriteLine($"There is no airport with IATA: {input}");
                }
            }
        }

        public string GetDescription()
        {
            return "Geo Location Service";
        }
    }
}
