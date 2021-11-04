﻿namespace Airports_Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Airports_Logic.Model;

    public class ExerciseResultsUtility : ICommand
    {
        private IDataHandler handler;

        public ExerciseResultsUtility(IDataHandler handler)
        {
            this.handler = handler;
        }

        public void Start()
        {
            var airports = this.handler.Airports;
            var cities = this.handler.Cities;
            var countries = this.handler.Countries;

            Console.WriteLine($"Total entries: airports: {airports.Count()}, cities: {cities.Count()}, countries: {countries.Count()}");

            this.ShowTaskA(airports, cities, countries);

            this.ShowTaskB(airports, cities, countries);

            this.ShowTaskC(airports, cities, countries);

            this.ShowTaskD(airports, cities, countries);

            this.ShowTaskE(airports, cities, countries);
        }

        public string GetDescription()
        {
            return "Shows the results of the queries on the input data.";
        }

        private void ShowTaskE(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            string pattern = @"[euioa]";
            Regex regex = new Regex(pattern);

            var query = from airport in airports
                        select new { airport.Name, Vowels = regex.Matches(airport.Name).Count() };

            var result = query.OrderByDescending(a => a.Vowels).First();

            Console.WriteLine($"The airport {result.Name} has the most vowels with {result.Vowels}");
        }

        private void ShowTaskD(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var query = from city in cities
                        join airport in airports on city.Id equals airport.CityId
                        select new { CityName = city.Name, AirportName = airport.Name };

            var longestAirportNameCity = query.OrderByDescending(c => c.AirportName.Length).First();
            Console.WriteLine($"The city with the longest airport name is: {longestAirportNameCity.CityName}. Airport name is: {longestAirportNameCity.AirportName}");
        }

        private void ShowTaskC(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var query = from country in countries
                        orderby country.Name descending
                        join airport in airports on country.Id equals airport.CountryId
                        group airport by country.Name;

            foreach (var countryAirports in query)
            {
                Console.WriteLine($"-- {countryAirports.Key} --");
                foreach (var airport in countryAirports)
                {
                    Console.WriteLine($"\t{airport.Name}");
                }
            }
        }

        private void ShowTaskB(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var citiesAndAirports = from airport in airports
                                    join city in cities on airport.CityId equals city.Id
                                    group airport by city.Name into cityairport
                                    orderby cityairport.Count() descending
                                    select new { Name = cityairport.Key, AirportsCount = cityairport.Count() };
            int maxCount = citiesAndAirports.First().AirportsCount;

            citiesAndAirports = citiesAndAirports.TakeWhile(ca => ca.AirportsCount == maxCount);

            foreach (var cityAirportItem in citiesAndAirports)
            {
                Console.WriteLine($"{cityAirportItem.Name} has {cityAirportItem.AirportsCount} airports.");
            }
        }

        private void ShowTaskA(List<Airport> airports, List<City> cities, List<Country> countries)
        {
            var countryAirportsQuery = from airport in airports
                                       join country in countries on airport.CountryId equals country.Id
                                       orderby country.Name ascending
                                       group airport by country.Name;

            foreach (var countryAirport in countryAirportsQuery)
            {
                foreach (var item in countryAirport)
                {
                    Console.WriteLine($"--> Country {countryAirport.Key} has {countryAirport.Count()} airports.");
                }
            }
        }
    }
}