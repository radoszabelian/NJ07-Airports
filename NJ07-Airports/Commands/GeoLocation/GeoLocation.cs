using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NJ07_Airports.Commands.GeoLocation
{
    public class GeoLocation : ICommand
    {
        private CacheAndDataHandler _handler;

        public GeoLocation(CacheAndDataHandler handler)
        {
            _handler = handler;
        }

        public string GetDescription()
        {
            return "GeoLocation";
        }

        public void Start()
        {
            //kérek vagy gps location-t vagy iata kódot
            Console.WriteLine("Please enter GPS Coordinate or IATA Code!");
            string input = Console.ReadLine();


            //ha megfelelő a formátuma
            //Regex gpsCoordinateSample = new Regex(@"^\be\d*.\d*\b\, \b\d*.\d*\b, \d*$");
            Regex gpsCoordinateSample = new Regex(@"^\-?\d*\.\d*\, \-?\d*.\d*, \d*$");
            Regex iataSample = new Regex(@"^\b[A-Z0-9]{2,3}\b$");

            //akkor gps loc esetén add vissza a legközelebbi airportot
            if (gpsCoordinateSample.IsMatch(input)) {
                WriteNearestAirportByGPSCoordinates(input);
            }

            //iata kód esetén add vissza az adott airportot
            if (iataSample.IsMatch(input))
            {
                WriteAirportByIATA(input);
            }
        }

        private void WriteNearestAirportByGPSCoordinates(string input)
        {
            //1. szedd szét elemeire
            string[] coordinates = input.Split(", ");
            Coordinate origin = new Coordinate(Convert.ToDouble(coordinates[0]), Convert.ToDouble(coordinates[1]));

            //2. hasonlítsd össze az összes airporttal és keresd ki a minimumot (min keresés)
            var closestAirport =  _handler.Airports.Select(airport => new
            {
                Distance =
                GeoCalculator.GetDistance(origin,
                new Coordinate(
                    Convert.ToDouble(airport.Location.Latitude),
                    Convert.ToDouble(airport.Location.Longitude)
                    )
                ),
                Airport = airport
            }
            ).OrderBy(a => a.Distance).First();


            //3. írd ki az airport adatait
            Console.WriteLine($"The closest airport is: {closestAirport.Airport.Name} - Distance: {closestAirport.Distance}");
        }

        private void WriteAirportByIATA(string input)
        {
            var airport = _handler.Airports.First(a => a.IATACode == input);

            Console.WriteLine($"The airport with the IATA Code {input} is: {airport.Name}");
        }
    }
}
