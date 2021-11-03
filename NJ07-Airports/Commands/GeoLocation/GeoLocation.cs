namespace NJ07_Airports.Commands.GeoLocation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Geolocation;

    /// <summary>
    /// This Class serves as an executable feature, it asks for a geo location or iata code and
    /// displays the closest airport to the coordinates.
    /// </summary>
    public class GeoLocation : ICommand
    {
        private ICacheAndDataHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLocation"/> class.
        /// </summary>
        /// <param name="handler">CacheAndDataHandler for getting the data.</param>
        public GeoLocation(ICacheAndDataHandler handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Used to write the description of this class where needed.
        /// </summary>
        /// <returns>The string description.</returns>
        public string GetDescription()
        {
            return "GeoLocation";
        }

        /// <summary>
        /// Starts the execution of this feature class.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Please enter GPS Coordinate or IATA Code!");
            string input = Console.ReadLine();

            Regex gpsCoordinateSample = new Regex(@"^\-?\d*\.\d*\, \-?\d*.\d*, \d*$");
            Regex iataSample = new Regex(@"^\b[A-Z0-9]{2,3}\b$");

            if (gpsCoordinateSample.IsMatch(input))
            {
                this.WriteNearestAirportByGPSCoordinates(input);
            }

            if (iataSample.IsMatch(input))
            {
                this.WriteAirportByIATA(input);
            }
        }

        private void WriteNearestAirportByGPSCoordinates(string input)
        {
            string[] coordinates = input.Split(", ");
            Coordinate origin = new Coordinate(Convert.ToDouble(coordinates[0]), Convert.ToDouble(coordinates[1]));

            var closestAirport = this.handler.Airports.Select(airport => new
            {
                Distance =
                GeoCalculator.GetDistance(origin, new Coordinate(
                    Convert.ToDouble(airport.Location.Latitude),
                    Convert.ToDouble(airport.Location.Longitude))),
                Airport = airport,
            }).OrderBy(a => a.Distance).First();

            Console.WriteLine($"The closest airport is: {closestAirport.Airport.Name} - Distance: {closestAirport.Distance}");
        }

        private void WriteAirportByIATA(string input)
        {
            var airport = this.handler.Airports.First(a => a.IATACode == input);

            Console.WriteLine($"The airport with the IATA Code {input} is: {airport.Name}");
        }
    }
}
