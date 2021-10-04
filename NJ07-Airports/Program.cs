using NJ07_Airports.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NJ07_Airports
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Airport> airports = new List<Airport>();
            List<City> cities = new List<City>();
            List<Country> countries = new List<Country>();

            CsvHelper.ParseAirportFileAndFillLists(airports, cities, countries);

            var airlines = CsvHelper.Parse<Airline>(@"data/airlines.dat");
            var flights = CsvHelper.Parse<Flight>(@"data/flights.dat");

            ExerciseResultsUtility.ShowResults(airports, cities, countries);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(airlines.Count());
            Console.WriteLine(flights.Count());
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}
